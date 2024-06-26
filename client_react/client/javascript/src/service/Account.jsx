import { patchAPI, deleteAPI, postAPI } from "./BaseService";

export const updateAccountName = async (accountID, accountName) => {
  try {
    const requestBody = {
      accountName: accountName  // Assuming accountName is already a string
    };

    const response = await patchAPI(`/api/account/${accountID}`, requestBody);
    return response.data;
  } catch (error) {
    throw error;
  }
};


export const deleteAccount = async (accountID) => {
  try {
    const response = await deleteAPI(`/api/account/${accountID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createAccount = async (account, type) => {
  if (type === "Deposit") {
    const response = await postAPI("api/account/createDepositAccount", account);
    return response;
  } else if (type === "Withdrawal") {
    console.log("burada");
    const response = await postAPI("api/account/createCheckingAccount", account);
    return response;
  }
};

export const updateAccountBalance = async (accountID, newBalance) => {
  try {
    const requestBody = {
      balance: newBalance  // Assuming newBalance is a number
    };

    const response = await patchAPI(`/api/account/${accountID}/updateBalance`, requestBody);
    return response.data;
  } catch (error) {
    throw error;
  }
};
