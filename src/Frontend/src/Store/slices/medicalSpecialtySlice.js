import { createSlice } from "@reduxjs/toolkit";

export const medicalSpecialtySlice = createSlice({
    name: "medicalSpecialty",
    initialState: {
        medicalSpecialtyData: null,
    },
    reducers: {
        storeMedicalSpecialtyData: (state, action) => {
            state.medicalSpecialtyData = action.payload;
            localStorage.setItem("medicalSpecialtyData", JSON.stringify(action.payload));
        },
        updateMedicalSpecialty: (state, action) => {
            const index = state.medicalSpecialtyData.findIndex(
                (c) => c.id === action.payload.id
            );
            if (index !== -1) {
                state.medicalSpecialtyData[index] = {
                    ...state.medicalSpecialtyData[index],
                    ...action.payload,
                };
                // Updating local storage with updated data
                localStorage.setItem(
                    "medicalSpecialtyData",
                    JSON.stringify(state.medicalSpecialtyData)
                );
            }
        },
        deleteMedicalSpecialty: (state, action) => {
            state.medicalSpecialtyData = state.medicalSpecialtyData.filter(
                (medicalSpecialty) => medicalSpecialty.id !== action.payload
            );
            // Updating local storage with updated data
            localStorage.setItem(
                "medicalSpecialtyData",
                JSON.stringify(state.medicalSpecialtyData)
            );
        },
    },
});

export const { storeMedicalSpecialtyData, updateMedicalSpecialty, deleteMedicalSpecialty } = medicalSpecialtySlice.actions;
export default medicalSpecialtySlice.reducer;