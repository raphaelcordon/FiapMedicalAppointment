import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./Layout";
import Home from "./Home";
import NotFound from "./NotFound";
import ProtectedRoutes from "./ProtectedRoutes";
import Login from "./Login";
import Registration from "./Registration/index.jsx";
import Doctor from "./Doctor/index.jsx";
import Patient from "./Patient/index.jsx";
import MedicalSpecialty from "./MedicalSpecialty/index.jsx";
import AppointmentDoctor from "./AppointmentDoctor/index.jsx";
import AppointmentPatient from "./AppointmentPatient/index.jsx";
const Router = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Layout />}>
                    <Route path="/" element={<Home />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/registration" element={<Registration />} />

                    <Route element={<ProtectedRoutes />}>
                        <Route path="/doctor" element={<Doctor />} />
                        <Route path="/patient" element={<Patient />} />
                        <Route path="/medicalspecialty" element={<MedicalSpecialty />} />
                        <Route path="/appointmentdoctor" element={<AppointmentDoctor />} />
                        <Route path="/appointmentpatient" element={<AppointmentPatient />} />
                    </Route>
                </Route>

                <Route path="*" element={<NotFound />} />
            </Routes>
        </BrowserRouter>
    );
};

export default Router;