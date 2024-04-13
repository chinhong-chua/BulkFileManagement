export class fileManipulationModel {
    static toRequestJson (model){
        return {
            FolderPath: model.folderPath,
            Files: model.files,
            IncludeSubfolders: model.includeSubfolders,
            Manipulations: model.manipulations,
        }
    }

    static toResponseJson (model){
        return {
            folderPath: model.FolderPath,
            files: model.Files,
            includeSubfolders: model.IncludeSubfolders,
            manipulations: model.Manipulations,
        }
    }
}