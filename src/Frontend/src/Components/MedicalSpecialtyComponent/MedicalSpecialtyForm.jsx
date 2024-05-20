import { useState } from "react";
import { PostMedicalSpecialty, UpdateMedicalSpecialty } from "../../axios/medicalSpecialty.js";
import PropTypes from 'prop-types';

const MedicalSpecialtyForm = ({ specialty, onSave, onCancel }) => {
  const [specialtyName, setSpecialtyName] = useState(specialty ? specialty.specialty : '');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    try {
      if (specialty) {
        await UpdateMedicalSpecialty(specialty.id, { specialty: specialtyName });
      } else {
        await PostMedicalSpecialty({ specialty: specialtyName });
      }
      onSave();
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-4">
        <label className="block mb-2 text-sm text-accent-content">Specialty Name</label>
        <input
          type="text"
          value={specialtyName}
          onChange={(e) => setSpecialtyName(e.target.value)}
          className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
          required
        />
      </div>
      {error && <p className="text-error text-sm mt-2">{error}</p>}
      <div className="flex justify-between">
        <button type="submit" className="btn btn-primary">
          {specialty ? 'Update' : 'Add'} Specialty
        </button>
        {specialty && (
          <button type="button" onClick={onCancel} className="btn btn-secondary">
            Cancel
          </button>
        )}
      </div>
    </form>
  );
};

MedicalSpecialtyForm.propTypes = {
  specialty: PropTypes.shape({
    id: PropTypes.string,
    specialty: PropTypes.string,
  }),
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func,
};

export default MedicalSpecialtyForm;
