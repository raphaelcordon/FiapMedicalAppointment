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

export const GetMedicalSpecialtyById = async (id) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get(`/MedicalSpecialty/${id}/`, config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const GetAllMedicalSpecialties = async () => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get("/MedicalSpecialty/", config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch data");
    }
};

export const PostMedicalSpecialty = async (data) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.post("/MedicalSpecialty/", data, config);
        return res.data;
    } catch (error) {
        console.error("Registration error: ", error.response?.data?.message || "Failed to register specialty");
        throw new Error(error.response?.data?.message || "No account found");
    }
};

export const DeleteMedicalSpecialty = async (id) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.delete(`/MedicalSpecialty/${id}/`, config);
        return res.data;
    } catch (error) {
        throw new Error("Fail to delete, please try again");
    }
};

export const UpdateMedicalSpecialty = async (id, data) => {
    const config = getAxiosConfig();
    try {
        const res = await axios.put(`/MedicalSpecialty/${id}/`, data, config);
        return res.data;
    } catch (err) {
        console.error(err);
        throw err;
    }
};

export const PostMedicalSpecialtyToAUser = async (userId, specialtyId) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.post(`/MedicalSpecialty/users/${userId}/specialties/${specialtyId}/`, {}, config);
        return res.data;
    } catch (error) {
        console.error("Registration error: ", error.response?.data?.message || "Failed to register specialty");
        throw new Error(error.response?.data?.message || "No account found");
    }
};

export const DeleteMedicalSpecialtyFromUser = async (userId, specialtyId) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.delete(`/MedicalSpecialty/users/${userId}/specialties/${specialtyId}/`, config);
        return res.data;
    } catch (error) {
        console.error("Removal error: ", error.response?.data?.message || "Failed to remove specialty");
        throw new Error(error.response?.data?.message || "No account found");
    }
};