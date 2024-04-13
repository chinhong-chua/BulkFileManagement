import axios from 'axios';

const API_URL = 'http://localhost:5139/filemanipulation'; 

export const manipulateFiles = async (files, manipulations) => {
  try {
    const response = await axios.post(API_URL, {
      files,
      manipulations
    });
    return response.data;
  } catch (error) {
    console.error('Failed to manipulate files:', error);
    throw error;
  }
};
