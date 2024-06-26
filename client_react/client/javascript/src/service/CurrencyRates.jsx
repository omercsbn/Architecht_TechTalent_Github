import { getAPI } from "./BaseService";

// Tüm döviz kurlarını getirme fonksiyonu
export const getAllCurrencyRates = async () => {
  try {
    const response = await getAPI("/api/currencyrate");
    return response.data;
  } catch (error) {
    throw error;
  }
};
