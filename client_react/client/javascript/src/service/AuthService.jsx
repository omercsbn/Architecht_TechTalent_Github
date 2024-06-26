import { postAPI, deleteAPI, getAPI } from "./BaseService";

export async function Login(data) {
  const url = "/api/user/login";
  const apiData = {
    email: data.email,
    password: data.password,
  };
  return await postAPI(url, apiData);
}

export async function Register(data) {
  const url = "/api/user/create";
  const apiData = {
    name: data.name,
    surname: data.surname,
    email: data.email,
    password: data.password,
    Phone: "+90 555 555 55 55",
    Address: ""

  };
  return await postAPI(url, apiData);
}

export async function Logout() {
  const url = "/api/users/logout";
  return await deleteAPI(url);
}

export async function getDarkModeStatus() {
  const url = "/api/users/darkmode";
  return await getAPI(url);
}

export async function setDarkModeStatus() {
  const url = "/api/users/darkmode";
  return await postAPI(url);
}

export async function deleteUserAccount(userId) {
  const url = `/api/user/delete/${userId}`;
  return await deleteAPI(url);
}