import { createSlice } from "@reduxjs/toolkit";

export const appointmentSlice = createSlice({
    name: "appointment",
    initialState: {
        appointmentData: [],
    },
    reducers: {
        storeAppointmentData: (state, action) => {
            state.appointmentData = action.payload;
            localStorage.setItem("appointmentData", JSON.stringify(action.payload));
        },
        addAppointment: (state, action) => {
            state.appointmentData.push(action.payload);
            localStorage.setItem("appointmentData", JSON.stringify(state.appointmentData));
        },
        updateAppointment: (state, action) => {
            const index = state.appointmentData.findIndex(
                (c) => c.id === action.payload.id
            );
            if (index !== -1) {
                state.appointmentData[index] = {
                    ...state.appointmentData[index],
                    ...action.payload,
                };
                localStorage.setItem(
                    "appointmentData",
                    JSON.stringify(state.appointmentData)
                );
            }
        },
        deleteAppointment: (state, action) => {
            state.appointmentData = state.appointmentData.filter(
                (appointment) => appointment.id !== action.payload
            );
            localStorage.setItem(
                "appointmentData",
                JSON.stringify(state.appointmentData)
            );
        },
    },
});

export const { storeAppointmentData, addAppointment, updateAppointment, deleteAppointment } = appointmentSlice.actions;
export default appointmentSlice.reducer;
