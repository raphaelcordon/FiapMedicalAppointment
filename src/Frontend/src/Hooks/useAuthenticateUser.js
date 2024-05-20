import {useDispatch} from "react-redux";
import {useCallback, useState} from "react";
import {loginUser, storeUserData} from "../Store/slices/userSlice.js";
import {AuthenticateUser} from "../axios/account.js";

const useAuthenticateUser = () => {
    const dispatch = useDispatch();
    const [error, setError] = useState(null);

    const authenticateUser = useCallback(async (email, password) => {
        setError(null);
        try {
            const authResponse = await AuthenticateUser(email, password);
            window.localStorage.setItem("token", authResponse.token);
            dispatch(loginUser(authResponse.token));
            dispatch(storeUserData(authResponse.user));

        } catch (error) {
            setError(error.message || "An error occurred during login.");
            throw error;
        }
    }, [dispatch]);

    return { authenticateUser, error };
};
export default useAuthenticateUser;