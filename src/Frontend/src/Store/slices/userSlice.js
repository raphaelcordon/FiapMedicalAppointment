import { createSlice } from "@reduxjs/toolkit";

export const userSlice = createSlice({
    name: "user",
    initialState: {
        token: window.localStorage.getItem('token') || null,
        userData: null,
    },
    reducers: {
        loginUser: (state, action) => {
            state.token = action.payload;
        },
        logoutUser: (state) => {
            console.log('Logging out user');
            state.token = null;
            state.userData = null;
        },
        storeUserData: (state, action) => {
            state.userData = action.payload;
        },
    },
});

export const { loginUser, logoutUser, storeUserData } = userSlice.actions;
export default userSlice.reducer;