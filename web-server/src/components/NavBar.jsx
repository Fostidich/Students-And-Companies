import { NavLink } from 'react-router-dom';

const Navbar = () => {
    return (
        <nav className="bg-white p-4 flex items-center justify-between w-full shadow-md px-9 fixed top-0 left-0 z-50">
            <div className="flex items-center">
                <img onClick={() => window.location.href = "/"}
                    src="/src/assets/logo.svg"
                    alt="Site Logo"
                    className="w-20 h-20 rounded-full mr-4"
                />
            </div>
            <div className="flex-1 mx-4">
                <input
                    type="text"
                    placeholder="Search an internship"
                    className="w-full px-4 py-2 rounded-lg text-gray-700 border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>
            <div className="flex space-x-4">
                <NavLink
                    to="/"
                    className={({ isActive }) => isActive ? "font-bold text-[#2C2C2C]" : "text-[#6C757D] hover:text-[#2C2C2C]"}
                >
                    Home
                </NavLink>
                <NavLink
                    to="/profile"
                    className={({ isActive }) => isActive ? "font-bold text-[#2C2C2C]" : "text-[#6C757D] hover:text-[#2C2C2C]"}
                >
                    Profile
                </NavLink>
                <NavLink
                    to="/notification"
                    className={({ isActive }) => isActive ? "font-bold text-[#2C2C2C]" : "text-[#6C757D] hover:text-[#2C2C2C]"}
                >
                    Notification
                </NavLink>
                <NavLink
                    to="/help"
                    className={({ isActive }) => isActive ? "font-bold text-[#2C2C2C]" : "text-[#6C757D] hover:text-[#2C2C2C]"}
                >
                    Help
                </NavLink>
            </div>
        </nav>
    );
};

export default Navbar;