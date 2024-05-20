import { useEffect, useState } from "react";
import { GetAllMedicalSpecialties, DeleteMedicalSpecialty } from "../../axios/medicalSpecialty.js";
import MedicalSpecialtyForm from "./MedicalSpecialtyForm.jsx";
import PropTypes from 'prop-types';

const MedicalSpecialtyList = () => {
  const [specialties, setSpecialties] = useState([]);
  const [editingSpecialty, setEditingSpecialty] = useState(null);
  const [isAdding, setIsAdding] = useState(false);

  useEffect(() => {
    fetchSpecialties();
  }, []);

  const fetchSpecialties = async () => {
    try {
      const data = await GetAllMedicalSpecialties();
      setSpecialties(data);
    } catch (error) {
      console.error("Failed to fetch specialties", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await DeleteMedicalSpecialty(id);
      fetchSpecialties();
    } catch (error) {
      console.error("Failed to delete specialty", error);
    }
  };

  const handleSave = () => {
    setEditingSpecialty(null);
    setIsAdding(false);
    fetchSpecialties();
  };

  return (
    <div className="mt-6">
      <h1 className="mb-4">Medical Specialties</h1>
      <button onClick={() => setIsAdding(true)} className="btn btn-primary mb-4">
        Add New Specialty
      </button>
      {isAdding && (
        <MedicalSpecialtyForm onSave={handleSave} onCancel={() => setIsAdding(false)} />
      )}
      <ul className="list-disc pl-5">
        {specialties.map((specialty) => (
          <li key={specialty.id} className="mb-2">
            {specialty.specialty}
            <button
              onClick={() => setEditingSpecialty(specialty)}
              className="btn btn-secondary ml-4"
            >
              Edit
            </button>
            <button
              onClick={() => handleDelete(specialty.id)}
              className="btn btn-danger ml-2"
            >
              Delete
            </button>
            {editingSpecialty && editingSpecialty.id === specialty.id && (
              <MedicalSpecialtyForm
                specialty={editingSpecialty}
                onSave={handleSave}
                onCancel={() => setEditingSpecialty(null)}
              />
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default MedicalSpecialtyList;
