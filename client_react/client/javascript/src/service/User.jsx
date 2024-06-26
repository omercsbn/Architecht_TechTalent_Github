import { getAPI, putAPI } from "./BaseService";

export const getUserAccounts = async (userID) => {
  try {
    const response = await getAPI(`/api/account/${userID}/accounts`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getAllUserIbans = async () => {
  try {
    const response = await getAPI("/api/users/accounts");
    const users = response.data.data;
    const IBANs = users.map(user => user.iban); // Her bir kullanıcının IBAN'ını alın
    return IBANs;
  } catch (error) {
    throw error;
  }
};

// Kullanıcının adını ve soyadını getiren fonksiyon
export const getNameSurname = async (userID) => {
  try {
    const response = await getAPI(`/api/user/${userID}/namesurname`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Fatura sayısını getiren fonksiyon
export const getInvoiceCount = async () => {
  try {
    const response = await getAPI("/api/invoices/count");
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Hesap sayısını getiren fonksiyon
export const getAccountCount = async () => {
  try {
    const response = await getAPI("/api/account/totalAccountCount");
    return response.data;
  } catch (error) {
    throw error;
  }
};

// İşlem sayısını getiren fonksiyon
export const getTransactionCount = async () => {
  try {
    const response = await getAPI("/api/transactions/totalTransactionCount");
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Son 5 işlemi getiren fonksiyon
export const getRecentTransactions = async () => {
  try {
    const response = await getAPI("/api/transactions/recent");
    return response.data;
  } catch (error) {
    throw error;
  }
};

// IBAN'a göre hesabı getiren fonksiyon
export const getAccountByIBAN = async (iban) => {
  try {
    const response = await getAPI(`/api/invoices/account/${iban}`); // API'nin doğru olduğunu ve IBAN'a göre hesapları getirebildiğini varsayalım
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getUserProfile = async (userId) => {
  try {
    const response = await getAPI(`/api/user/${userId}`); // API endpointini doğru olduğunu ve kullanıcı profili verilerini getirebildiğini varsayalım
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateUserProfile = async (userId, userData) => {
  try {
    const response = await putAPI(`/api/user/update/${userId}`, userData);
    return response.data; // Güncellenmiş kullanıcı profili verisini döndür
  } catch (error) {
    throw error;
  }
};