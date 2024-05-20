import AppointmentPatientList from "../../Components/AppointmentComponent/AppointmentPatientList.jsx";
import ScheduleAppointmentComponent from "../../Components/AppointmentComponent/ScheduleAppointment.jsx";

const AppointmentPatient = () => {
  return (
    <section className="">
      <h1>Appointment Patient</h1>
      <ScheduleAppointmentComponent />
      <AppointmentPatientList />
    </section>
  );
};

export default AppointmentPatient;
