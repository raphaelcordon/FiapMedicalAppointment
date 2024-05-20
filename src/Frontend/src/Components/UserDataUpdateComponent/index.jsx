import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import Button from "../SmallComponents/Button.jsx";
import { useEffect, useState } from "react";
import useGetMeUser from "../../Hooks/useGetMeUser.js";
import { UpdateMeUser } from "../../axios/user.js";
import PropTypes from 'prop-types';

const UserDataUpdate = ({ user }) => {
  const [email, setEmail] = useState('');
  const [address, setAddress] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const { getUser } = useGetMeUser();

  useEffect(() => {
    if (user) {
      setEmail(user.email || '');
      setAddress(user.address || '');
      setPhoneNumber(user.phoneNumber || '');
    }
  }, [user]);
  const handleUpdateData = async (e) => {
    e.preventDefault();
    setError('');

    const userData = { email, address, phoneNumber };

    try {
      setIsLoading(true);
      await UpdateMeUser(user.id, userData);
      await getUser();
      setSuccess(true);
      setError(null);
      setTimeout(() => {
        setSuccess(false); // Hide the success message
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
    <div className="flex xl:items-center l:items-center justify-center sm:mt-50 md:mt-50">
      <div className="max-w-md w-full p-6 bg-base-100 rounded-lg shadow-lg">
        {success && (
            <div className="success-overlay">
              <div className="text-center p-10 bg-base-100/70 rounded-lg">
                <FontAwesomeIcon icon={faCheck} className="text-8xl text-secondary"/>
                <h2 className="mt-8 mb-6">Profile successfully updated</h2>
              </div>
            </div>
        )}
        {error && <small>{String(error)}</small>}
        <h1 className="mb-4">Personal data</h1>
        <form onSubmit={handleUpdateData}>
          <div className="mb-4">

            <div className="mb-2">
              <label htmlFor="email" className="block text-center mb-2 text-sm text-accent-content">
                E-Mail
              </label>
              <input
                  type="email"
                  id="email"
                  name="email"
                  value={email}
                  disabled
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary text-center"
              />
            </div>
            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Address</label>
              <input
                  name="address"
                  id="address"
                  type="text"
                  value={address}
                  onChange={(e) => setAddress(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>
            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Phone Number</label>
              <input
                  name="phoneNumber"
                  id="phoneNumber"
                  type="tel"
                  value={phoneNumber}
                  onChange={(e) => setPhoneNumber(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>
            <Button
                type="submit"
                disabled={isLoading || email === "" || address === "" || phoneNumber === ""}
            >
              {isLoading ? 'Saving Profile...' : 'Save Profile'}
            </Button>
            {error && <p className="text-error text-sm mt-2">{error}</p>}
          </div>
        </form>
      </div>
    </div>
  );
};

UserDataUpdate.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.string,
    email: PropTypes.string,
    address: PropTypes.string,
    phoneNumber: PropTypes.string,
  }),
};

export default UserDataUpdate;
