import { type ReactElement } from "react";
import { Navigate, Outlet } from "react-router-dom";

export const PrivateWrapper = (): ReactElement => {
  return localStorage.getItem("token") != null ? (
    <Outlet />
  ) : (
    <Navigate to="/login" />
  );
};
