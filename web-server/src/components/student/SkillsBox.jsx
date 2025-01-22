import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;

const SkillsBox = () => {
    const [skills, setSkills] = useState([]);
    const [newSkill, setNewSkill] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const authData = (() => {
        try {
            return JSON.parse(Cookies.get('authData'));
        } catch {
            return null;
        }
    })();

    if (!authData) {
        return <div>Please log in to manage your skills.</div>;
    }

    // eslint-disable-next-line react-hooks/rules-of-hooks
    useEffect(() => {
        fetchSkills();
    }, []);

    const fetchSkills = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch(`${API_SERVER_URL}/api/profile/skills`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });
            if (response.ok) {
                const data = await response.json();
                setSkills(data);
            }
            if (response.status === 404) {
                setSkills([]);
                //return;
            }

        } catch (err) {
            console.error('Error fetching skills:', err);
            setError(null);
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!newSkill.trim()) return;

        setError(null);
        try {
            const skillObject = { name: newSkill };
            const response = await fetch(`${API_SERVER_URL}/api/profile/skills`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authData.token}`,
                },
                body: JSON.stringify(skillObject),
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setNewSkill('');
            fetchSkills();
        } catch (err) {
            console.error('Error adding skill:', err);
            setError('Failed to add skill');
        }
    };

    const deleteSkill = async (idSkill) => {
        setError(null);
        try {
            const response = await fetch(`${API_SERVER_URL}/api/profile/skills/delete/${idSkill}`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setSkills((prevSkills) => prevSkills.filter((skill) => skill.id !== idSkill));
            fetchSkills();
        } catch (err) {
            console.error('Error deleting skill:', err);
            setError('Failed to delete skill');
        }
    };

    return (
        <div className="w-full">
            <h3 className="text-xl font-bold mb-4">Skills</h3>
            {error && <div className="text-red-500 mb-2">{error}</div>}
            <form onSubmit={handleSubmit} className="flex gap-2 mb-4">
                <input
                    type="text"
                    value={newSkill}
                    onChange={(e) => setNewSkill(e.target.value)}
                    className="border rounded p-2 flex-grow"
                    placeholder="Add a new skill"
                />
                <button
                    type="submit"
                    className={`bg-blue-500 text-white px-4 py-2 rounded ${loading ? 'opacity-50' : ''}`}
                    disabled={loading}
                >
                    {loading ? 'Adding...' : 'Add'}
                </button>
            </form>
            {skills.length > 0 ? (
                <div className="flex flex-wrap gap-2">
                    {skills.map((skill) => (
                        <span key={skill.id} className="bg-gray-200 rounded-full px-3 py-1 flex items-center">
                            {skill.name}
                            <button
                                onClick={() => deleteSkill(skill.id)}
                                className="ml-2 text-red-500"
                            >
                                x
                            </button>
                        </span>
                    ))}
                </div>
            ) : (
                <div>No skills found. Add your first skill!</div>
            )}
        </div>
    );
};

export default SkillsBox;