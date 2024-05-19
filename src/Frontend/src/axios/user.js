import AxiosMotion from "./_base.js";

const axios = AxiosMotion;

const getAxiosConfig = () => {
    const token = window.localStorage.getItem("token");
    const headers = {
        Authorization: `Bearer ${token}`,
    };
    const config = {
        headers,
    };
    return config;
};

export const GetMeUser = async () => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get("/user/me/", config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const GetUserById = async (id) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get(`/user/${id}/`, config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const GetAllUsers = async () => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get("/user/", config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const GetAllUsersByRole = async (role) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get(`/user/role/${role}/`, config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const DeleteUser = async (id) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.delete(`/user/${id}/`, config);
        return res.data;
    } catch (error) {
        throw new Error("Fail to delete, please try again");
    }
};

export const UpdateMeUser = async (id, data) => {
    const config = getAxiosConfig();
    try {
        const res = await axios.put(`/user/${id}/`, data, config);
        return res.data;
    } catch (err) {
        console.error(err);
        throw err;
    }
};
