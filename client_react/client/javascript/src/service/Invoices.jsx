import { getAPI, postAPI, putAPI } from "./BaseService";

// Yeni bir fatura oluşturma fonksiyonu
export const createInvoice = async (invoiceData) => {
  try {
    // postAPI ile POST isteği gönderiyoruz
    const response = await postAPI("/api/invoices", invoiceData);
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Toplam fatura sayısını getiren fonksiyon
export const getTotalInvoiceNumber = async () => {
  try {
    // getAPI ile GET isteği gönderiyoruz
    const response = await getAPI("/api/invoices/totalcount");
    return response.data; // API yanıtından totalInvoiceNumber değerini döndür
  } catch (error) {
    throw error;
  }
};

// Tüm faturaları getiren fonksiyon
export const getInvoices = async (userID) => {
  try {
    const response = await getAPI(`/api/invoices?userID=${userID}`); // GET isteği gönder
    return response.data; // Fatura verilerini döndür
  } catch (error) {
    throw error;
  }
};

// Tüm faturaları getiren fonksiyon
export const doPayment = async (invoiceID) => {
  try {
    const response = await putAPI(`/api/invoices/${invoiceID}`); // GET isteği gönder
    return response.data; // Fatura verilerini döndür
  } catch (error) {
    throw error;
  }
};

