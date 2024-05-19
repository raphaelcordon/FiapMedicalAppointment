import {configureStore} from '@reduxjs/toolkit'
import userReducer from './slices/userSlice.js';
import medicalSpecialtyReducer from "./slices/medicalSpecialtySlice.js";

export default configureStore({
    reducer: {
        user: userReducer,
        medicalSpecialty: medicalSpecialtyReducer
    },
},)