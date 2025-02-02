import {useEffect, useState} from "react";
import Cookies from "js-cookie";
import CompanyNotification from "../components/company/CompanyNotification.jsx";
import {getErrorMessage} from "../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function ApplicationsNotifications() {
    const authData = JSON.parse(Cookies.get('authData'));
    const [adv, setAdv] = useState([]);
    const [pendingApplications, setPendingApplications] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setIsLoading(true);
                // Fetch advertisements
                const advResponse = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements`, {
                    headers: {
                        'Authorization': `Bearer ${authData.token}`,
                    },
                });

                if (!advResponse.ok) {
                    const message = await getErrorMessage(advResponse);
                    setErrorMessage(message);
                    throw new Error(message);
                }

                const advertisementData = await advResponse.json();
                setAdv(advertisementData);

                // Fetch applications for each advertisement
                const applications = [];

                for (const ad of advertisementData) {
                    try {
                        const appResponse = await fetch(`${API_SERVER_URL}/api/enrollment/applications/${ad.advertisementId}`, {
                            headers: {
                                'Authorization': `Bearer ${authData.token}`,
                            }
                        });

                        // Gestione esplicita dello status 404
                        if (appResponse.status === 404) {
                            console.log(`No applications for ad ${ad.advertisementId}`);
                            continue;
                        }

                        if (!appResponse.ok) {
                            const message = await getErrorMessage(appResponse);
                            console.error(`Error fetching applications for ad ${ad.advertisementId}:`, message);
                            continue;
                        }

                        const appData = await appResponse.json();
                        if (Array.isArray(appData) && appData.length > 0) {
                            applications.push(...appData);
                        }
                    } catch (error) {
                        console.error(`Error processing ad ${ad.advertisementId}:`, error);
                    }
                }

                setPendingApplications(applications);
            } catch (error) {
                console.error('Error fetching data:', error);
                setErrorMessage(error.message);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, []);

    const handleNotification = (applicationId) => {
        setPendingApplications(prev => prev.filter(application => application.applicationId !== applicationId));
    };

    return (
        <div className="p-8">
            <div className="pt-8 px-4 flex justify-center">
                <div className="grid grid-cols-1 gap-4 w-full">
                    {isLoading ? (
                        <div className="text-center">Loading...</div>
                    ) : errorMessage ? (
                        <div className="text-red-500 text-center">{errorMessage}</div>
                    ) : pendingApplications.length > 0 ? (
                        pendingApplications.map((pendingApplication) => (
                            <CompanyNotification
                                key={pendingApplication.applicationId}
                                pendingApplication={pendingApplication}
                                handleChange={handleNotification}
                            />
                        ))
                    ) : (
                        <div className="text-center">No pending applications</div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default ApplicationsNotifications;