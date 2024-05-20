import { useDispatch } from "react-redux";
import { useCallback, useState } from "react";
import { storeMedicalSpecialtyData } from "../Store/slices/medicalSpecialtySlice.js";
import { GetAllMedicalSpecialties } from "../axios/medicalSpecialty.js";

const useGetMedicalSpecialties = () => {
    const dispatch = useDispatch();
    const [error, setError] = useState(null);

    const getMedicalSpecialties = useCallback(async () => {
        setError(null);
        try {
            const res = await GetAllMedicalSpecialties();
            dispatch(storeMedicalSpecialtyData(res));
        } catch (error) {
            setError(error.message || "An error occurred retrieving medical specialties.");
        }
    }, [dispatch]);

    return { getMedicalSpecialties, error };
};

export default useGetMedicalSpecialties;
