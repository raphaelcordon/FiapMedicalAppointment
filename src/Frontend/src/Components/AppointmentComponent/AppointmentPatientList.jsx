import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetAppointmentsForPatient, UpdateAppointmentStatus, CancelAppointment } from "../../axios/appointment";

const AppointmentPatientList = () => {
  const user = useSelector((state) => state.user.userData);
  const [appointments, setAppointments] = useState([]);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const data = await GetAppointmentsForPatient(user.id);
        setAppointments(data);
      } catch (err) {
        setError(err.message);
      }
    };

    if (user && user.id) {
      fetchAppointments();
    }
  }, [user]);

  const pastAppointments = appointments.filter(appointment => new Date(appointment.appointmentTime) < new Date());
  const futureAppointments = appointments.filter(appointment => new Date(appointment.appointmentTime) >= new Date());

  const handleUpdate = async (appointmentId, newStatus) => {
    try {
      await UpdateAppointmentStatus(appointmentId, newStatus);
      setSuccess("Appointment updated successfully");
      setError("");
      // Refresh appointments
      const data = await GetAppointmentsForPatient(user.id);
      setAppointments(data);
    } catch (err) {
      setError(err.message);
      setSuccess("");
    }
  };

  const handleDelete = async (appointmentId) => {
    try {
      await CancelAppointment(appointmentId);
      setSuccess("Appointment canceled successfully");
      setError("");
      // Refresh appointments
      const data = await GetAppointmentsForPatient(user.id);
      setAppointments(data);
    } catch (err) {
      setError(err.message);
      setSuccess("");
    }
  };

  return (
    <div className="p-6 bg-base-200 rounded-lg">
      <h2 className="text-xl font-semibold text-accent mb-4">Future Appointments</h2>
      {futureAppointments.length > 0 ? (
        futureAppointments.map(appointment => (
          <div key={appointment.id} className="mb-4 p-4 border rounded-lg shadow-sm bg-base-100">
            <p className="text-lg">
              <span className="font-semibold">{appointment.specialty}</span> with
              <span className="font-semibold"> {appointment.doctorName}</span> on
              <span className="font-semibold"> {new Date(appointment.appointmentTime).toLocaleString()}</span>
            </p>
            <div className="flex justify-end space-x-2 mt-2">
              <button
                onClick={() => handleUpdate(appointment.id, "Reschedule")}
                className="btn btn-primary btn-sm"
              >
                Update
              </button>
              <button
                onClick={() => handleDelete(appointment.id)}
                className="btn btn-error btn-sm"
              >
                Cancel
              </button>
            </div>
          </div>
        ))
      ) : (
        <p>No future appointments.</p>
      )}

      <h2 className="text-xl font-semibold text-accent mb-4 mt-6">Past Appointments</h2>
      {pastAppointments.length > 0 ? (
        pastAppointments.map(appointment => (
          <div key={appointment.id} className="mb-4 p-4 border rounded-lg shadow-sm bg-base-100">
            <p className="text-lg">
              <span className="font-semibold">{appointment.specialty}</span> with
              <span className="font-semibold"> {appointment.doctorName}</span> on
              <span className="font-semibold"> {new Date(appointment.appointmentTime).toLocaleString()}</span>
            </p>
          </div>
        ))
      ) : (
        <p>No past appointments.</p>
      )}

      {error && <p className="text-error text-sm mt-2">{error}</p>}
      {success && <p className="text-success text-sm mt-2">{success}</p>}
    </div>
  );
};

export default AppointmentPatientList;
