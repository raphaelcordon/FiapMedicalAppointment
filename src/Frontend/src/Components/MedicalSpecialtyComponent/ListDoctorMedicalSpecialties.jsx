import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { useEffect, useState } from "react";
import useGetMeUser from "../../Hooks/useGetMeUser.js";
import { PostMedicalSpecialtyToAUser, DeleteMedicalSpecialtyFromUser } from "../../axios/medicalSpecialty.js";
import PropTypes from 'prop-types';

const ListDoctorMedicalSpecialties = ({ user, medicalSpecialty }) => {
  const [specialtyList, setSpecialtyList] = useState([]);
  const [selectedSpecialties, setSelectedSpecialties] = useState([]);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const { getUser } = useGetMeUser();

  useEffect(() => {
    if (medicalSpecialty) {
      // Sort specialties alphabetically
      const sortedSpecialties = [...medicalSpecialty].sort((a, b) => a.specialty.localeCompare(b.specialty));
      setSpecialtyList(sortedSpecialties);
    }
  }, [medicalSpecialty]);

  useEffect(() => {
    if (user && user.medicalSpecialties) {
      const userSpecialties = user.medicalSpecialties.map(specialty =>
        typeof specialty === 'string' ? specialty : specialty.specialty
      );
      setSelectedSpecialties(userSpecialties);
    }
  }, [user]);

  const handleCheckboxChange = async (specialtyObj, isChecked) => {
    setError('');
    setIsLoading(true);

    try {
      if (isChecked) {
        await PostMedicalSpecialtyToAUser(user.id, specialtyObj.id);
      } else {
        await DeleteMedicalSpecialtyFromUser(user.id, specialtyObj.id);
      }
      await getUser();
      setSuccess(true);
      setTimeout(() => {
        setSuccess(false);
      }, 1000);
    } catch (error) {
      if (error.response && error.response.data && error.response.data.message) {
        setError(error.response.data.message);
      } else {
        setError("An error occurred while updating the profile.");
      }
      console.log(error);
    } finally {
      setIsLoading(false);
    }
  };

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className="flex xl:items-center l:items-center justify-center sm:mt-50 md:mt-50 mb-5">
      <div className="max-w-md w-full p-6 bg-base-100 rounded-lg shadow-lg">
        {success && (
          <div className="success-overlay">
            <div className="text-center p-10 bg-base-100/70 rounded-lg">
              <FontAwesomeIcon icon={faCheck} className="text-8xl text-secondary" />
              <h2 className="mt-8 mb-6">Profile successfully updated</h2>
            </div>
          </div>
        )}
        {error && <small>{String(error)}</small>}
        <div className="mb-4">
          <div className="mb-2">
            <label className="block mb-4 text-sm text-accent-content">Medical Specialties</label>
            <div className="grid grid-cols-3 gap-4">
              {specialtyList.map((specialtyObj) => (
                <div key={specialtyObj.id} className="mb-2">
                  <label className="inline-flex items-center">
                    <input
                      type="checkbox"
                      name="medicalSpecialty"
                      value={specialtyObj.specialty}
                      checked={selectedSpecialties.includes(specialtyObj.specialty)}
                      onChange={(e) => handleCheckboxChange(specialtyObj, e.target.checked)}
                      className="form-checkbox"
                    />
                    <span className="ml-2">{specialtyObj.specialty}</span>
                  </label>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

ListDoctorMedicalSpecialties.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.string,
    email: PropTypes.string,
    address: PropTypes.string,
    phoneNumber: PropTypes.string,
    medicalSpecialties: PropTypes.arrayOf(PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        id: PropTypes.string,
        specialty: PropTypes.string
      })
    ])),
  }),
  medicalSpecialty: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string.isRequired,
    specialty: PropTypes.string.isRequired,
  })),
};

export default ListDoctorMedicalSpecialties;
