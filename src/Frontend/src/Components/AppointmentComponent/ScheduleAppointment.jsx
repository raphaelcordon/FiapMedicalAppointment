import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetAllMedicalSpecialties } from "../../axios/medicalSpecialty";
import { GetAllAppointmentSpans, ScheduleAppointment } from "../../axios/appointment";
import { GetAllUsersByRole } from "../../axios/user";

const ScheduleAppointmentComponent = () => {
  const user = useSelector((state) => state.user.userData);
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
        setSpans(spansData);
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
          console.log("Fetched doctors:", doctorsData);

          const selectedSpecialtyName = specialties.find(spec => spec.id === selectedSpecialty)?.specialty;

          const filteredDoctors = doctorsData.filter(doctor => {
            console.log(`Doctor ${doctor.email}'s specialties:`, doctor.medicalSpecialties);
            return doctor.medicalSpecialties.includes(selectedSpecialtyName);
          });

          console.log("Filtered doctors:", filteredDoctors);
          setDoctors(filteredDoctors);
          setSelectedDoctor(""); // Clear selected doctor when specialty changes
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
      await ScheduleAppointment(data);
      setSuccess(true);
      setError("");
    } catch (err) {
      setError(err.message);
      setSuccess(false);
    }
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
          <button type="submit" className="btn btn-primary">
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
