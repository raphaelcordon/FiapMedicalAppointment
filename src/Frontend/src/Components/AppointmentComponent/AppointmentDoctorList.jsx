import { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { GetAppointmentsForDoctor, UpdateAppointmentStatus, CancelAppointment, GetAllAppointmentSpans } from "../../axios/appointment";
import { deleteAppointment, storeAppointmentData, updateAppointment } from "../../Store/slices/appointmentSlice.js";

const AppointmentDoctorList = () => {
  const user = useSelector((state) => state.user.userData);
  const appointments = useSelector((state) => state.appointment.appointmentData);
  const dispatch = useDispatch();
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [isEditing, setIsEditing] = useState(false);
  const [currentAppointment, setCurrentAppointment] = useState(null);
  const [newAppointmentTime, setNewAppointmentTime] = useState("");
  const [newSpan, setNewSpan] = useState("");
  const [newStatus, setNewStatus] = useState("");
  const [spans, setSpans] = useState([]);

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const data = await GetAppointmentsForDoctor(user.id);
        dispatch(storeAppointmentData(data));
      } catch (err) {
        setError(err.message);
      }
    };

    if (user && user.id) {
      fetchAppointments();
    }
  }, [user, dispatch]);

  useEffect(() => {
    const fetchSpans = async () => {
      try {
        const spansData = await GetAllAppointmentSpans();
        spansData.sort((a, b) => a.duration - b.duration);
        setSpans(spansData);
        if (spansData.length > 0) {
          setNewSpan(spansData[0].id); // Set default span
        }
      } catch (err) {
        setError(err.message);
      }
    };

    fetchSpans();
  }, []);

  const pastAppointments = appointments.filter(appointment => new Date(appointment.appointmentTime) < new Date());
  const futureAppointments = appointments.filter(appointment => new Date(appointment.appointmentTime) >= new Date());

  const handleUpdateClick = (appointment) => {
    setIsEditing(true);
    setCurrentAppointment(appointment);
    setNewAppointmentTime(new Date(appointment.appointmentTime).toISOString().slice(0, 16));
    setNewSpan(appointment.spanId || spans[0]?.id);
    setNewStatus(appointment.status);
  };

  const handleUpdate = async () => {
    try {
      const updateData = {
        newAppointmentTime,
        newSpan,
        newStatus
      };
      const updatedAppointment = await UpdateAppointmentStatus(currentAppointment.id, updateData);
      dispatch(updateAppointment(updatedAppointment));
      setSuccess("Appointment updated successfully");
      setError("");
      setIsEditing(false);
      setCurrentAppointment(null);
    } catch (err) {
      setError(err.message);
      setSuccess("");
    }
  };

  const handleDelete = async (appointmentId) => {
    try {
      await CancelAppointment(appointmentId);
      dispatch(deleteAppointment(appointmentId));
      setSuccess("Appointment canceled successfully");
      setError("");
    } catch (err) {
      setError(err.message);
      setSuccess("");
    }
  };

  const handleCancelEdit = () => {
    setIsEditing(false);
    setCurrentAppointment(null);
  };

  return (
    <div className="p-6 bg-base-200 rounded-lg">
      <h2 className="text-xl font-semibold text-accent mb-4">Future Appointments</h2>
      {futureAppointments.length > 0 ? (
        futureAppointments.map(appointment => (
          <div key={appointment.id} className="mb-4 p-4 border rounded-lg shadow-sm bg-base-100">
            <p className="text-lg">
              <span className="font-semibold">{appointment.specialty}</span> with
              <span className="font-semibold"> {appointment.patientName}</span> on
              <span className="font-semibold"> {new Date(appointment.appointmentTime).toLocaleString()}</span>
            </p>
            <div className="flex justify-end space-x-2 mt-2">
              <button
                onClick={() => handleUpdateClick(appointment)}
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
              <span className="font-semibold"> {appointment.patientName}</span> on
              <span className="font-semibold"> {new Date(appointment.appointmentTime).toLocaleString()}</span>
            </p>
          </div>
        ))
      ) : (
        <p>No past appointments.</p>
      )}

      {isEditing && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg w-1/2">
            <h2 className="text-2xl font-bold mb-4">Update Appointment</h2>
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700">Appointment Time:</label>
              <input
                type="datetime-local"
                value={newAppointmentTime}
                onChange={(e) => setNewAppointmentTime(e.target.value)}
                className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
              />
            </div>
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700">Duration:</label>
              <select
                value={newSpan}
                onChange={(e) => setNewSpan(e.target.value)}
                className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
              >
                {spans.map(span => (
                  <option key={span.id} value={span.id}>{span.duration} minutes</option>
                ))}
              </select>
            </div>
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700">Status:</label>
              <input
                type="text"
                value={newStatus}
                onChange={(e) => setNewStatus(e.target.value)}
                className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
              />
            </div>
            <div className="flex justify-end space-x-2">
              <button
                onClick={handleUpdate}
                className="btn btn-primary"
              >
                Save
              </button>
              <button
                onClick={handleCancelEdit}
                className="btn btn-secondary"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}

      {error && <p className="text-error text-sm mt-2">{error}</p>}
      {success && <p className="text-success text-sm mt-2">{success}</p>}
    </div>
  );
};

export default AppointmentDoctorList;
