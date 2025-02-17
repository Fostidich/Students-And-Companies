import Cookies from "js-cookie";
import { useState, useEffect } from "react";
import { getErrorMessage } from "../../utils/errorUtils.js";
const API_SERVER_URL =
  window.env?.VITE_API_SERVER_URL || "http://localhost:5000";

function MyInternship() {
  const [status, setStatus] = useState({
    loading: true,
    error: null,
  });
  const [internships, setInternships] = useState([]);
  const [advertisements, setAdvertisements] = useState({});
  const [showOldInternships, setShowOldInternships] = useState(false);
  const [feedbackForm, setFeedbackForm] = useState({
    isOpen: false,
    internshipId: null,
    rating: 1,
    comment: "",
    error: null,
  });
  const [feedbacks, setFeedbacks] = useState({});
  const [companyFeedbacks, setCompanyFeedbacks] = useState({});

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const internshipsData = await getInternships();
      if (!internshipsData || internshipsData.length === 0) {
        setStatus({ loading: false, error: null });
        return;
      }

      // Fetch advertisement details for all internships
      const advPromises = internshipsData.map((internship) =>
        getInternshipDetails(internship.advertisementId),
      );
      const advResults = await Promise.all(advPromises);

      const advMap = {};
      advResults.forEach((adv, index) => {
        if (adv) {
          advMap[internshipsData[index].advertisementId] = adv;
        }
      });

      setInternships(internshipsData);
      setAdvertisements(advMap);
      setStatus({ loading: false, error: null });
    } catch (error) {
      setStatus({ loading: false, error: error.message });
    }
  };

  const getInternships = async () => {
    const authData = JSON.parse(Cookies.get("authData"));
    const response = await fetch(`${API_SERVER_URL}/api/internship`, {
      headers: {
        Authorization: `Bearer ${authData.token}`,
      },
    });

    if (!response.ok) {
      throw new Error(await getErrorMessage(response));
    }

    return await response.json();
  };

  const getInternshipDetails = async (advertisementId) => {
    const authData = JSON.parse(Cookies.get("authData"));
    const response = await fetch(
      `${API_SERVER_URL}/api/recommendation/advertisements/${advertisementId}`,
      {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      },
    );

    if (!response.ok) return null;
    return await response.json();
  };

  const getCompanyFeedback = async (internshipId) => {
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
    setCompanyFeedbacks((prev) => ({ ...prev, [internshipId]: feedback }));
  };

  const getFeedback = async (internshipId) => {
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
    setFeedbacks((prev) => ({ ...prev, [internshipId]: feedback }));
  };

  const submitFeedback = async (e) => {
    e.preventDefault();
    const authData = JSON.parse(Cookies.get("authData"));

    try {
      const response = await fetch(
        `${API_SERVER_URL}/api/internship/${feedbackForm.internshipId}/feedback/student`,
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

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const isInternshipActive = (endDate) => {
    return new Date(endDate) > new Date();
  };

  const handleFeedbackClick = async (internshipId) => {
    if (!feedbacks[internshipId]) {
      await getFeedback(internshipId);
      await getCompanyFeedback(internshipId);
    }
    setFeedbackForm({
      isOpen: true,
      internshipId,
      rating: 1,
      comment: "",
      error: null,
    });
  };

  if (status.loading) return <div className="p-4">Loading...</div>;

  if (status.error) {
    return (
      <div className="bg-white rounded-lg border p-4 mb-4">
        <p className="text-red-500">{status.error}</p>
      </div>
    );
  }

  const activeInternship = internships.find((internship) =>
    isInternshipActive(internship.endDate),
  );
  const pastInternships = internships.filter(
    (internship) => !isInternshipActive(internship.endDate),
  );

  return (
    <div className="columns-1 w-full h-full space-y-4 p-4">
      {activeInternship && (
        <div className="bg-white rounded-lg p-4 mb-4 shadow">
          <h1 className="text-xl font-semibold mb-4">
            Your ongoing internship
          </h1>
          <div className="space-y-2">
            <p>
              <strong>Name:</strong>{" "}
              {advertisements[activeInternship.advertisementId]?.name}
            </p>
            <p>
              <strong>Description:</strong>{" "}
              {advertisements[activeInternship.advertisementId]?.description}
            </p>
            <p>
              <strong>Duration:</strong>{" "}
              {advertisements[activeInternship.advertisementId]?.duration}{" "}
              months
            </p>
            <p>
              <strong>Start Date:</strong>{" "}
              {formatDate(activeInternship.startDate)}
            </p>
            <p>
              <strong>End Date:</strong> {formatDate(activeInternship.endDate)}
            </p>
          </div>
        </div>
      )}

      {pastInternships.length > 0 && (
        <div
          className={
            activeInternship
              ? "bg-white rounded-lg p-4 shadow"
              : "bg-white rounded-lg p-4 shadow mb-4"
          }
        >
          <div>
            <button
              onClick={() =>
                setShowOldInternships(!showOldInternships) ||
                pastInternships.forEach((internship) =>
                  getFeedback(internship.internshipId),
                ) ||
                pastInternships.forEach((internship) =>
                  getCompanyFeedback(internship.internshipId),
                )
              }
              className=" px-4 py-2 rounded text-white bg-[#2c2c2c] hover:bg-[#1e1e1e] "
            >
              {showOldInternships ? "Hide" : "Show"} Past Internships
            </button>
          </div>

          {showOldInternships && (
            <div className="mt-4 space-y-4">
              {pastInternships.map((internship) => (
                <div
                  key={internship.internshipId}
                  className="bg-white rounded-lg p-4 shadow"
                >
                  <div className="space-y-2">
                    <p>
                      <strong>Name:</strong>{" "}
                      {advertisements[internship.advertisementId]?.name}
                    </p>
                    <p>
                      <strong>Description:</strong>{" "}
                      {advertisements[internship.advertisementId]?.description}
                    </p>
                    <p>
                      <strong>Duration:</strong>{" "}
                      {advertisements[internship.advertisementId]?.duration}{" "}
                      months
                    </p>
                    <p>
                      <strong>Start Date:</strong>{" "}
                      {formatDate(internship.startDate)}
                    </p>
                    <p>
                      <strong>End Date:</strong>{" "}
                      {formatDate(internship.endDate)}
                    </p>
                    {companyFeedbacks[internship.internshipId] && (
                      <div className="mt-2 p-2 bg-gray-50 rounded">
                        <h3 className="font-semibold">Company Feedback</h3>
                        <p>
                          Rating:{" "}
                          {companyFeedbacks[internship.internshipId].rating}/10
                        </p>
                        <p>
                          Comment:{" "}
                          {companyFeedbacks[internship.internshipId].comment}
                        </p>
                      </div>
                    )}

                    {feedbacks[internship.internshipId] ? (
                      <div className="mt-2 p-2 bg-gray-50 rounded">
                        <h3 className="font-semibold">Your Feedback</h3>
                        <p>
                          Rating: {feedbacks[internship.internshipId].rating}/10
                        </p>
                        <p>
                          Comment: {feedbacks[internship.internshipId].comment}
                        </p>
                      </div>
                    ) : (
                      <button
                        onClick={() =>
                          handleFeedbackClick(internship.internshipId)
                        }
                        className="mt-2 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
                      >
                        Feedback
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
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
                  className="px-4 py-2  rounded text-white bg-[#2c2c2c] hover:bg-[#1e1e1e]"
                >
                  Submit
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default MyInternship;
