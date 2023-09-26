import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { ClientList } from "../../components";
import { VehicleList } from "../../components/vehicleList";

export const Home = () => {
  const [currentTab, setCurrentTab] = useState(0);

  const navigate = useNavigate();

  return (
    <div className="w-full h-full flex flex-col">
      <div className="navbar bg-base-200">
        <div className="flex-1">
          <a className="btn btn-ghost normal-case text-xl">
            Sistema de aluguel de carros
          </a>
        </div>
        <div className="flex-none">
          <div className="dropdown dropdown-end">
            <label tabIndex={0} className="btn btn-ghost btn-circle avatar">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="w-6 h-6"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M17.982 18.725A7.488 7.488 0 0012 15.75a7.488 7.488 0 00-5.982 2.975m11.963 0a9 9 0 10-11.963 0m11.963 0A8.966 8.966 0 0112 21a8.966 8.966 0 01-5.982-2.275M15 9.75a3 3 0 11-6 0 3 3 0 016 0z"
                />
              </svg>
            </label>
            <ul
              tabIndex={0}
              className="menu menu-compact dropdown-content mt-3 p-2 shadow bg-base-200 rounded-box w-52"
            >
              <li>
                <a
                  onClick={() => {
                    localStorage.removeItem("token");

                    navigate("/login");
                  }}
                >
                  Logout
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div className="flex flex-col grow h-full">
        <div className="tabs mx-12 my-6">
          <a
            className={`tab tab-bordered tab-lg ${
              currentTab === 0 && "tab-active"
            }`}
            onClick={() => setCurrentTab(0)}
          >
            Cliente
          </a>
          <a
            className={`tab tab-bordered tab-lg ${
              currentTab === 1 && "tab-active"
            }`}
            onClick={() => setCurrentTab(1)}
          >
            Veículo
          </a>
        </div>
        {currentTab === 0 && <ClientList />}
        {currentTab === 1 && <VehicleList />}
      </div>
    </div>
  );
};
