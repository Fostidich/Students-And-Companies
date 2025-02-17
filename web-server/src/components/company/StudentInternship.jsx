import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { getErrorMessage } from "../../utils/errorUtils";
import PropTypes from "prop-types";
const API_SERVER_URL =
  window.env?.VITE_API_SERVER_URL || "http://localhost:5000";

function StudentInternship({ internship, isActive }) {
  const authData = JSON.parse(Cookies.get("authData"));
  const [feedbackForm, setFeedbackForm] = useState({
    isOpen: false,
    internshipId: null,
    rating: 1,
    comment: "",
    error: null,
  });
  const [feedbacks, setFeedbacks] = useState({});
  const [studentFeedbacks, setStudentFeedbacks] = useState({});
  const [errorMessage, setErrorMessage] = useState("");
  const [profileStudent, setProfileStudent] = useState({
    studentId: 0,
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
  useEffect(() => {
    const fetchData = async () => {
      await getInformation();
      await getFeedback(internship.internshipId);
      await getStudentFeedback(internship.internshipId);
    };
    fetchData();
  }, []);

  const getInformation = async () => {
    const response = await fetch(
      `${API_SERVER_URL}/api/profile/student/${internship.studentId}`,
      {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      },
    );
    if (response.status === 200) {
      const data = await response.json();
      setProfileStudent(data);
    } else {
      const message = await getErrorMessage(response);
      setErrorMessage(message);
      console.error("Error checking advertisement details:", errorMessage);
    }
  };
  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const getFeedback = async (internshipId) => {
    const authData = JSON.parse(Cookies.get("authData"));
    const response = await fetch(
      `${API_SERVER_URL}/api/internship/${internshipId}/feedback/company`,
      {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      },
    );

    if (!response.ok) return null;
    const feedback = await response.json();
    setFeedbacks((prev) => ({ ...prev, [internshipId]: feedback }));
  };
  const getStudentFeedback = async (internshipId) => {
    const authData = JSON.parse(Cookies.get("authData"));
    const response = await fetch(
      `${API_SERVER_URL}/api/internship/${internshipId}/feedback/student`,
      {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      },
    );

    if (!response.ok) return null;
    const feedback = await response.json();
    setStudentFeedbacks((prev) => ({ ...prev, [internshipId]: feedback }));
  };

  const submitFeedback = async (e) => {
    e.preventDefault();
    const authData = JSON.parse(Cookies.get("authData"));

    try {
      const response = await fetch(
        `${API_SERVER_URL}/api/internship/${feedbackForm.internshipId}/feedback/company`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${authData.token}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            rating: parseInt(feedbackForm.rating),
            comment: feedbackForm.comment,
          }),
        },
      );

      if (!response.ok) {
        throw new Error(await getErrorMessage(response));
      }

      await getFeedback(feedbackForm.internshipId);
      setFeedbackForm({
        isOpen: false,
        internshipId: null,
        rating: 1,
        comment: "",
        error: null,
      });
    } catch (error) {
      setFeedbackForm((prev) => ({ ...prev, error: error.message }));
    }
  };

  const handleFeedbackClick = async (internshipId) => {
    if (!feedbacks[internshipId]) {
      await getFeedback(internshipId);
      await getStudentFeedback(internshipId);
    }
    setFeedbackForm({
      isOpen: true,
      internshipId,
      rating: 1,
      comment: "",
      error: null,
    });
  };

  return (
    <div>
      <div className="bg-white p-4 rounded-lg shadow-md mb-4">
        <div className="space-y-2">
          <h3 className="text-lg font-semibold">
            {profileStudent.name} {profileStudent.surname}
          </h3>
          <p className="text-sm">
            <strong>University:</strong> {profileStudent.university}
          </p>
          <p className="text-sm">
            <strong>Course of Study:</strong> {profileStudent.courseOfStudy}
          </p>
          <p className="text-sm">
            <strong>Bio:</strong> {profileStudent.bio}
          </p>
          <div className="mt-2 border-t pt-2">
            <p className="text-xs text-gray-600">
              <strong>Email:</strong> {profileStudent.email}
            </p>
            <p className="text-xs text-gray-600">
              <strong>Internship start date:</strong>{" "}
              {formatDate(internship.startDate)}
            </p>
            <p className="text-xs text-gray-600">
              <strong>Internship end date:</strong>{" "}
              {formatDate(internship.endDate)}
            </p>
            <p className="text-xs text-gray-600">
              <strong>Internship created at:</strong>{" "}
              {formatDate(internship.createdAt)}
            </p>
          </div>
        </div>
        {errorMessage && (
          <div className="text-red-500 mt-4">{errorMessage}</div>
        )}
        {!isActive &&
          (feedbacks[internship.internshipId] ? (
            <div className="mt-2 p-2 bg-gray-50 rounded">
              <h3 className="font-semibold">Your Feedback</h3>
              <p>Rating: {feedbacks[internship.internshipId].rating}/10</p>
              <p>Comment: {feedbacks[internship.internshipId].comment}</p>
            </div>
          ) : (
            <button
              onClick={() => handleFeedbackClick(internship.internshipId)}
              className="mt-2 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
            >
              Feedback
            </button>
          ))}
        {!isActive && studentFeedbacks[internship.internshipId] && (
          <div className="mt-2 p-2 bg-gray-50 rounded">
            <h3 className="font-semibold">Student Feedback</h3>
            <p>Rating: {studentFeedbacks[internship.internshipId].rating}/10</p>
            <p>Comment: {studentFeedbacks[internship.internshipId].comment}</p>
          </div>
        )}
        {feedbackForm.isOpen && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <div className="bg-white p-6 rounded-lg w-96">
              <h2 className="text-xl font-bold mb-4">Provide Feedback</h2>
              <form onSubmit={submitFeedback} className="space-y-4">
                {feedbackForm.error && (
                  <p className="text-red-500">{feedbackForm.error}</p>
                )}
                <div>
                  <label className="block mb-2">Rating (1-10):</label>
                  <input
                    type="number"
                    min="1"
                    max="10"
                    value={feedbackForm.rating}
                    onChange={(e) =>
                      setFeedbackForm((prev) => ({
                        ...prev,
                        rating: e.target.value,
                      }))
                    }
                    className="border rounded p-2 w-full"
                    required
                  />
                </div>
                <div>
                  <label className="block mb-2">Comment:</label>
                  <textarea
                    value={feedbackForm.comment}
                    onChange={(e) =>
                      setFeedbackForm((prev) => ({
                        ...prev,
                        comment: e.target.value,
                      }))
                    }
                    className="border rounded p-2 w-full h-32"
                    required
                  />
                </div>
                <div className="flex justify-end space-x-2">
                  <button
                    type="button"
                    onClick={() =>
                      setFeedbackForm({
                        isOpen: false,
                        internshipId: null,
                        rating: 1,
                        comment: "",
                        error: null,
                      })
                    }
                    className="px-4 py-2 border rounded"
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                  >
                    Submit
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
StudentInternship.propTypes = {
  internship: PropTypes.shape({
    internshipId: PropTypes.number.isRequired,
    createdAt: PropTypes.string.isRequired,
    studentId: PropTypes.number.isRequired,
    companyId: PropTypes.number.isRequired,
    advertisementId: PropTypes.number.isRequired,
    startDate: PropTypes.string.isRequired,
    endDate: PropTypes.string.isRequired,
  }).isRequired,
  isActive: PropTypes.bool.isRequired,
};
export default StudentInternship;
