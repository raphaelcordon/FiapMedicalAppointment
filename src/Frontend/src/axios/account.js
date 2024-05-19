import AxiosMotion from "./_base.js";

const axios = AxiosMotion;

export const RegisterNewUser = async (data) => {
    try {
        await axios.post("/Account/register", data);
    } catch (error) {
        console.log('Registration error response:', error.response);
        throw new Error("Fail, please try again");
    }
}

export const AuthenticateUser = async (email, password) => {
    try {
        const res = await axios.post("/Account/login", { userName: email, password });
        return res.data;
    } catch (error) {
        console.error('Login error response:', error.response);
        throw new Error(error.response?.data?.message || "No account found");
    }
}