import { patchAPI, deleteAPI, postAPI, getAPI } from "./BaseService";

export const getGoldPrices = async () => {
  try {
    const response = await getAPI("/api/altin");
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createGoldAccount = async (data) => {
    try {
      const response = await postAPI("/api/altin/create", data); // data nesnesini isteğe ekleyelim
      return response.data;
    } catch (error) {
      throw error;
    }
  };