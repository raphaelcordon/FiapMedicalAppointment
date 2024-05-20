import { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { GetAllMedicalSpecialties } from "../../axios/medicalSpecialty";
import { GetAllAppointmentSpans, ScheduleAppointment, UpdateAppointmentStatus, CancelAppointment } from "../../axios/appointment";
import { addAppointment, updateAppointment, deleteAppointment, storeAppointmentData } from "../../Store/slices/appointmentSlice.js";

const ScheduleAppointmentComponent = () => {
  const user = useSelector((state) => state.user.userData);
  const dispatch = useDispatch();
  const [specialties, setSpecialties] = useState([]);
  const [spans, setSpans] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [selectedSpecialty, setSelectedSpecialty] = useState("");
  const [selectedDoctor, setSelectedDoctor] = useState("");
  const [selectedSpan, setSelectedSpan] = useState("");
  const [appointmentTime, setAppointmentTime] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const fetchSpecialtiesAndSpans = async () => {
      try {
        const specialtiesData = await GetAllMedicalSpecialties();
        setSpecialties(specialtiesData);

        const spansData = await GetAllAppointmentSpans();
        spansData.sort((a, b) => a.duration - b.duration); // Sort spans by duration
        setSpans(spansData);
        if (spansData.length > 0) {
          setSelectedSpan(spansData[0].id); // Set default selected span
        }
      } catch (err) {
        setError(err.message);
      }
    };

    fetchSpecialtiesAndSpans();
  }, []);

  useEffect(() => {
    const fetchDoctors = async () => {
      if (selectedSpecialty) {
        try {
          setDoctors([]);
          const doctorsData = await GetAllUsersByRole("Doctor");
          const selectedSpecialtyName = specialties.find(spec => spec.id === selectedSpecialty)?.specialty;

          const filteredDoctors = doctorsData.filter(doctor => {
            return doctor.medicalSpecialties.includes(selectedSpecialtyName);
          });

          setDoctors(filteredDoctors);
          setSelectedDoctor("");
        } catch (err) {
          setError(err.message);
        }
      } else {
        setDoctors([]);
      }
    };

    fetchDoctors();
  }, [selectedSpecialty, specialties]);

  const handleScheduleAppointment = async (e) => {
    e.preventDefault();
    try {
      const data = {
        doctorId: selectedDoctor,
        patientId: user.id,
        appointmentTime,
        spanId: selectedSpan,
        specialtyId: selectedSpecialty
      };
      const appointment = await ScheduleAppointment(data);
      dispatch(addAppointment(appointment));
      setSuccess(true);
      setError("");
    } catch (err) {
      setError(err.message);
      setSuccess(false);
    }
  };

  const getMinDate = () => {
    const date = new Date();
    date.setDate(date.getDate() + 1);
    return date.toISOString().slice(0, 16); // Format date as 'YYYY-MM-DDTHH:MM'
  };

  return (
    <div className="p-6 bg-white rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-4">Schedule New Appointment</h2>
      <form onSubmit={handleScheduleAppointment}>
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700">Specialty:</label>
          <select
            value={selectedSpecialty}
            onChange={(e) => setSelectedSpecialty(e.target.value)}
            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
            required
          >
            <option value="">Select Specialty</option>
            {specialties.map(specialty => (
              <option key={specialty.id} value={specialty.id}>{specialty.specialty}</option>
            ))}
          </select>
        </div>
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700">Doctor:</label>
          <select
            value={selectedDoctor}
            onChange={(e) => setSelectedDoctor(e.target.value)}
            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
            required
            disabled={selectedSpecialty === ""}
          >
            <option value="">Select Doctor</option>
            {doctors.length > 0 ? (
              doctors.map(doctor => (
                <option key={doctor.id} value={doctor.id}>{doctor.email}</option>
              ))
            ) : (
              <option value="" disabled>No doctors available</option>
            )}
          </select>
        </div>
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700">Appointment Time:</label>
          <input
            type="datetime-local"
            value={appointmentTime}
            onChange={(e) => setAppointmentTime(e.target.value)}
            min={getMinDate()}
            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
            required
          />
        </div>
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700">Duration:</label>
          <select
            value={selectedSpan}
            onChange={(e) => setSelectedSpan(e.target.value)}
            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
            required
          >
            {spans.map(span => (
              <option key={span.id} value={span.id}>{span.duration} minutes</option>
            ))}
          </select>
        </div>
        <div className="flex justify-between">
          <button type="submit" className="btn btn-primary" disabled={
            selectedSpecialty === "" ||
            selectedDoctor === "" ||
            appointmentTime === "" ||
            selectedSpan === ""
          }>
            Schedule Appointment
          </button>
        </div>
      </form>
      {error && <p className="text-error text-sm mt-2">{error}</p>}
      {success && <p className="text-success text-sm mt-2">Appointment scheduled successfully!</p>}
    </div>
  );
};

export default ScheduleAppointmentComponent;
