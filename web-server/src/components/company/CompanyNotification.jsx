import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { getErrorMessage } from "../../utils/errorUtils";
import PropTypes from "prop-types";
const API_SERVER_URL =
  window.env?.VITE_API_SERVER_URL || "http://localhost:5000";
function CompanyNotification({ pendingApplication, handleChange }) {
  const [errorMessage, setErrorMessage] = useState("");
  const authData = JSON.parse(Cookies.get("authData"));
  const [showDetails, setShowDetails] = useState(false);
  const [isLoadingStudent, setIsLoadingStudent] = useState(true);
  const [dateTime, setDateTime] = useState("");
  //const [isLoadingAd, setIsLoadingAd] = useState(true);
  const [profileStudent, setProfileStudent] = useState({
    studentId: "",
    username: "",
    email: "",
    bio: "",
    name: "",
    surname: "",
    university: "",
    courseOfStudy: "",
    gender: "",
    birthDate: "",
  });
  const [advertisement, setAdvertisement] = useState({
    advertisementId: "",
    name: "",
    createdAt: "",
    companyId: "",
    description: "",
    duration: "",
    spots: "",
    available: "",
    open: "",
    questionnaire: "",
  });

  useEffect(() => {
    const fetchData = async () => {
      await getStudentDetails();
      await getAdvertisementDetails();
    };
    fetchData();
  }, []);

  const getStudentDetails = async () => {
    setIsLoadingStudent(true);
    try {
      const response = await fetch(
        `${API_SERVER_URL}/api/profile/student/${pendingApplication.studentId}`,
        {
          headers: {
            Authorization: `Bearer ${authData.token}`,
          },
        },
      );

      if (!response.ok) {
        if (response.status === 404) {
          setErrorMessage("Student profile not found");
          return;
        }
        const message = await getErrorMessage(response);
        throw new Error(message);
      }

      const data = await response.json();
      setProfileStudent(data);
    } catch (error) {
      console.error("Error fetching student details:", error);
      setErrorMessage(error.message);
    } finally {
      setIsLoadingStudent(false);
    }
  };
  const getAdvertisementDetails = async () => {
    try {
      const response = await fetch(
        `${API_SERVER_URL}/api/recommendation/advertisements/${pendingApplication.advertisementId}`,
        {
          headers: {
            Authorization: `Bearer ${authData.token}`,
          },
        },
      );

      if (!response.ok) {
        if (response.status === 404) {
          setErrorMessage("Advertisement not found");
          return;
        }
        const message = await getErrorMessage(response);
        throw new Error(message);
      }

      const data = await response.json();
      setAdvertisement(data);
    } catch (error) {
      console.error("Error fetching advertisement details:", error);
      setErrorMessage(error.message);
    }
  };
  const getStudentCV = async () => {
    const response = await fetch(
      `${API_SERVER_URL}/api/profile/cv/${profileStudent.studentId}`,
      {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      },
    );
    if (response.status === 200) {
      // Converti la risposta in blob
      const blob = await response.blob();

      // Crea un URL temporaneo per il download
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;

      // Estrai il filename dall'header o usa un default
      const contentDisposition = response.headers.get("content-disposition");
      const filename = contentDisposition
        ? contentDisposition.split("filename=")[1].replace(/"/g, "")
        : `CV_studentId${profileStudent.studentId}.pdf`;

      a.download = filename;
      document.body.appendChild(a);
      a.click();

      // Pulizia
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } else {
      setErrorMessage(await getErrorMessage(response));
    }
  };
  const handleSubmit = async (e) => {
    e.preventDefault();
    await acceptApplication();
  };

  const acceptApplication = async () => {
    if (!dateTime) {
      setErrorMessage("Please select a date");
      return;
    }
    if (dateTime < new Date().toISOString().split("T")[0]) {
      setErrorMessage("Please select a future date");
      return;
    }
    setDateTime(new Date(dateTime).toISOString());

    const response = await fetch(
      `${API_SERVER_URL}/api/enrollment/accept/${pendingApplication.applicationId}`,
      {
        method: "POST",
        headers: {
          Authorization: `Bearer ${authData.token}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ dateTime }),
      },
    );
    if (response.status === 200) {
      console.log("Application accepted");
      setShowDetails(false);
      handleChange(pendingApplication.applicationId);
    } else {
      setErrorMessage("");
      setErrorMessage(await getErrorMessage(response));
    }
  };
  const rejectApplication = async () => {
    const response = await fetch(
      `${API_SERVER_URL}/api/enrollment/reject/${pendingApplication.applicationId}`,
      {
        method: "POST",
        headers: {
          Authorization: `Bearer ${authData.token}`,
          "Content-Type": "application/json",
        },
      },
    );
    if (response.status === 200) {
      console.log("Application accepted");
      setShowDetails(false);
      handleChange(pendingApplication.applicationId);
    } else {
      setErrorMessage("");
      setErrorMessage(await getErrorMessage(response));
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  return (
    <div className="bg-white rounded-lg border p-4 mb-4">
      {isLoadingStudent && <div className="text-center">Loading...</div>}
      <div className="flex justify-between items-start">
        <div>
          <h1 className="text-xl font-semibold mb-2">{advertisement.name}</h1>
          <h2 className="text-lg font-semibold mb-2">
            {profileStudent.name + " " + profileStudent.surname}
          </h2>
          <p className="text-xl font-light mb-2">{advertisement.description}</p>
        </div>
        <button
          onClick={() => setShowDetails(true)}
          className=" px-4 py-2 rounded text-white bg-[#2c2c2c] hover:bg-[#1e1e1e]"
        >
          View Details
        </button>
      </div>

      {showDetails &&
        ((isLoadingStudent && (
          <div className="text-center">Loading...</div>
        )) || (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative gap-4">
              <button
                onClick={() => setShowDetails(false) || setErrorMessage("")}
                className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
              >
                ✕
              </button>
              <h2 className="text-xl font-bold mb-4">
                Student profile details
              </h2>
              <div className=" justify-center space-y-2">
                <p>
                  <strong>Name:</strong> {profileStudent.name}
                </p>
                <p>
                  <strong>Surname:</strong> {profileStudent.surname}
                </p>
                <p>
                  <strong>University:</strong>{" "}
                  {profileStudent.university +
                    " " +
                    profileStudent.courseOfStudy}
                </p>
                <p>
                  <strong>Questionnaire response:</strong>{" "}
                  {pendingApplication.questionnaire}
                </p>
                <p>
                  <strong>Created:</strong>{" "}
                  {formatDate(pendingApplication.createdAt)}
                </p>
                <p>
                  <strong>Status:</strong> {pendingApplication.status}
                </p>
                <button
                  className="text-white bg-[#2c2c2c] hover:bg-[#1e1e1e] p-2 rounded-md "
                  onClick={getStudentCV}
                >
                  Download cv
                </button>
                <div></div>
              </div>

              <form
                onSubmit={handleSubmit}
                className={"flex flex-col gap-4 shadow-xl p-4 rounded-md"}
              >
                <div className={"flex flex-col gap-4 border p-4 rounded-md"}>
                  <label className="text-black flex justify-center items-center font-bold">
                    Start date
                  </label>
                  <input
                    type="date"
                    value={dateTime}
                    placeholder={"Start date"}
                    onChange={(e) => setDateTime(e.target.value)}
                    className="mb-4 px-4 py-2 border rounded-md"
                  />
                </div>

                <div className="flex justify-between grid grid-cols-2 gap-4">
                  <button
                    //onClick={() => acceptApplication()}
                    type={"submit"}
                    className="bg-green-500 text-white p-2 rounded-md hover:bg-green-600 "
                  >
                    Accept
                  </button>
                  <button
                    onClick={() => rejectApplication()}
                    className="bg-red-500 text-white p-2 rounded-md hover:bg-red-600 "
                  >
                    Reject
                  </button>
                </div>
              </form>
              <div></div>
              {errorMessage && (
                <div className="text-red-500 mt-4 mb-0">{errorMessage}</div>
              )}
            </div>
          </div>
        ))}
    </div>
  );
}
CompanyNotification.propTypes = {
  pendingApplication: PropTypes.shape({
    advertisementId: PropTypes.number.isRequired,
    applicationId: PropTypes.number.isRequired,
    createdAt: PropTypes.string.isRequired,
    questionnaire: PropTypes.string.isRequired,
    status: PropTypes.string.isRequired,
    studentId: PropTypes.number.isRequired,
  }),
  handleChange: PropTypes.func.isRequired,
};
export default CompanyNotification;
