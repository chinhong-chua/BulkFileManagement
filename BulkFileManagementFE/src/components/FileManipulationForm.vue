<template>
  <div class="container mt-5">
    <form @submit.prevent="submitForm" class="shadow p-4 rounded">
      <div class="row mb-3">
        <div class="col-md-12">
          <label for="folderPath" class="form-label">Folder Path:</label>
          <input
            type="text"
            id="folderPath"
            v-model="form.folderPath"
            class="form-control"
            placeholder="Enter or paste folder path"
          />
          <div v-if="validationErrors.FolderPath" class="text-danger">
            <ul>
              <li v-for="error in validationErrors.FolderPath" :key="error">
                {{ error }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="row mb-3">
        <div class="col-md-12">
          <label for="files" class="form-label">Select Files:</label>
          <input
            type="text"
            id="files"
            v-model="form.files"
            class="form-control"
            placeholder="Enter files separated by comma"
          />
          <!-- <input
            type="file"
            id="files"
            @change="handleFileSelection"
            class="form-control"
            multiple
          /> -->
          <div v-if="validationErrors.Files" class="text-danger">
            <ul>
              <li v-for="error in validationErrors.Files" :key="error">
                {{ error }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="row mb-3">
        <div class="col-md-12">
          <label class="form-label">Manipulations:</label>
          <div
            v-for="type in manipulationTypes"
            :key="type"
            class="form-check m-3"
          >
            <input
              type="checkbox"
              :id="`manipulation-${type}`"
              class="form-check-input"
              :value="type"
              @change="handleManipulationChange($event, type)"
            />
            <label :for="`manipulation-${type}`" class="form-check-label">{{
              type
            }}</label>
            <input
              v-if="
                type === 'ChangeExtension' && manipulationTexts.ChangeExtension
              "
              type="text"
              v-model="changeExtensionValue"
              class="form-control mt-2"
              placeholder="Enter new extension, e.g., .txt"
            />
            <input
              v-if="type === 'AddPrefix' && manipulationTexts.AddPrefix"
              type="text"
              v-model="addPrefixValue"
              class="form-control mt-2"
              placeholder="Enter prefix"
            />
            <input
              v-if="
                type === 'RemoveMatchingBegin' &&
                manipulationTexts.RemoveMatchingBegin
              "
              type="text"
              v-model="removeMatchingBeginValue"
              class="form-control mt-2"
              placeholder="Enter matching string from beginning"
            />
            <div
              class="row"
              v-if="type === 'ReplaceText' && manipulationTexts.ReplaceText"
            >
              <div class="col-md-6">
                <input
                  type="text"
                  v-model="replaceTextValue.oldText"
                  class="form-control mt-2"
                  placeholder="Enter text in filename to replace"
                />
              </div>
              <div class="col-md-6">
                <input
                  type="text"
                  v-model="replaceTextValue.newText"
                  class="form-control mt-2"
                  placeholder="Enter new text"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="row mb-3">
        <div class="col-md-6">
          <div class="form-check mb-3">
            <input
              type="checkbox"
              id="includeSubfolders"
              v-model="form.includeSubfolders"
              class="form-check-input"
            />
            <label for="includeSubfolders" class="form-check-label"
              >Include Subfolders?</label
            >
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-md-6">
          <div v-if="generalErrors.length" class="text-danger">
            <label class="form-label">Generic Error from backend:</label>
            <ul>
              <li v-for="error in generalErrors" :key="error">
                {{ error }}
              </li>
            </ul>
          </div>
        </div>
        <div class="col-md-6">
          <button type="submit" class="btn btn-primary float-end">
            Submit
          </button>
        </div>
      </div>
    </form>
    <SuccessModal
      :show="showSuccessModal"
      @update:show="showSuccessModal = $event"
    />
    <!-- for testing -->
    <!-- <button @click="() => (showSuccessModal = true)">Submit</button> -->
  </div>
</template>

<script setup>
import { ref, reactive } from "vue";
import axios from "axios";
import SuccessModal from "./SuccessModal.vue";

const form = ref({
  folderPath: "",
  files: "",
  // manipulations: [],
  includeSubfolders: false,
});

const validationErrors = ref({});
const generalErrors = ref([]);

const showSuccessModal = ref(false);

const manipulationTypes = [
  "AddPrefix",
  "ChangeExtension",
  "ToUpper",
  "RemoveMatchingBegin",
  "ReplaceText",
];
const manipulationTexts = reactive({
  AddPrefix: false,
  ChangeExtension: false,
  ToUpper: false,
  RemoveMatchingBegin: false,
  ReplaceText: false,
});

const changeExtensionValue = ref("");
const addPrefixValue = ref("");
const removeMatchingBeginValue = ref("");
const replaceTextValue = reactive({
  oldText: "",
  newText: "",
});

const handleFileSelection = (event) => {
  form.value.files = Array.from(event.target.files).map((file) => file.name);
};

const handleManipulationChange = (event, manipulationType) => {
  const { checked } = event.target;

  switch (manipulationType) {
    case "AddPrefix":
      manipulationTexts.AddPrefix = checked;
      addPrefixValue.value = "";
      break;
    case "ChangeExtension":
      manipulationTexts.ChangeExtension = checked;
      changeExtensionValue.value = "";
      break;
    case "ToUpper":
      manipulationTexts.ToUpper = checked;
      break;
    case "RemoveMatchingBegin":
      manipulationTexts.RemoveMatchingBegin = checked;
      removeMatchingBeginValue = "";
      break;
    case "ReplaceText":
      manipulationTexts.ReplaceText = checked;
      replaceTextValue.oldText = "";
      replaceTextValue.newText = "";
      break;
    default:
      break;
  }
};
const submitForm = async () => {
  try {
    clearErrors();

    let manipulations = [];
    if (manipulationTexts.AddPrefix && addPrefixValue.value)
      manipulations.push(`AddPrefix:${addPrefixValue.value}`);
    if (manipulationTexts.ChangeExtension && changeExtensionValue.value)
      manipulations.push(`ChangeExtension:${changeExtensionValue.value}`);
    if (manipulationTexts.ToUpper) manipulations.push(`ToUpper`);
    if (manipulationTexts.RemoveMatchingBegin)
      manipulations.push(
        `RemoveMatchingBegin:${removeMatchingBeginValue.value}`
      );
    if (manipulationTexts.ReplaceText)
      manipulations.push(
        `ReplaceText:${replaceTextValue.oldText}:${replaceTextValue.newText}`
      );

    let filesArray = form.value.files ? form.value.files.split(",") : [];

    const requestData = {
      FolderPath: form.value.folderPath,
      Files: filesArray,
      IncludeSubfolders: form.value.includeSubfolders,
      Manipulations: manipulations,
    };
    const headers = {
      "Content-Type": "application/json",
    };
    const url = "http://localhost:5139/filemanipulation";

    const response = await axios.post(url, requestData, { headers });
    const result = response.data;
    console.log(result);

    if (result && result[0]?.startsWith("Error")) {
      generalErrors.value = result;
      return;
    }
    showSuccessModal.value = true;
  } catch (error) {
    console.log(error);
    if (error.response.data.errors) {
      //Set validation errors
      const err = error.response.data.errors;
      console.error("err:", error);

      if (err.toString().startsWith("Error")) {
        generalErrors.value = err;
      } else {
        validationErrors.value = err;
      }
      console.error("Error submitting form:", validationErrors.value);
    }
  }
};

const clearErrors = () => {
  validationErrors.value = {};
  generalErrors.value = [];
};
</script>

<style>
.container {
  max-width: 800px;
  padding: 20px;
}
</style>
