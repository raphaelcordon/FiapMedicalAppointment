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

export const GetAppointmentsForDoctor = async (doctorId) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get(`/Appointment/doctor/${doctorId}`, config);
        return res.data;
    } catch (error) {
        throw new Error(error.response?.data?.message || "Failed to fetch appointments for doctor");
    }
};

export const GetAppointmentsForPatient = async (patientId) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get(`/Appointment/patient/${patientId}`, config);
        return res.data;
    } catch (error) {
        throw new Error(error.response?.data?.message || "Failed to fetch appointments for patient");
    }
};

export const ScheduleAppointment = async (data) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.post(`/Appointment/schedule`, data, config);
        return res.data;
    } catch (error) {
        console.error("Scheduling error: ", error.response?.data?.message || "Failed to schedule appointment");
        throw new Error(error.response?.data?.message || "No account found");
    }
};

export const UpdateAppointmentStatus = async (appointmentId, status) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.post(`/Appointment/update/${appointmentId}`, { newStatus: status }, config);
        return res.data;
    } catch (error) {
        throw new Error(error.response?.data?.message || "Failed to update appointment status");
    }
};

export const CancelAppointment = async (appointmentId) => {
    try {
        const config = getAxiosConfig();
        const res = await axios.post(`/Appointment/cancel/${appointmentId}`, {}, config);
        return res.data;
    } catch (error) {
        throw new Error(error.response?.data?.message || "Failed to cancel appointment");
    }
};

export const GetAllAppointmentSpans = async () => {
    try {
        const config = getAxiosConfig();
        const res = await axios.get("/AppointmentSpan", config);
        return res.data;
    } catch (error) {
        throw new Error("Not possible to fetch appointment spans");
    }
};
