import { type ReactElement } from "react";
import { Route, Routes } from "react-router-dom";

import { PrivateWrapper } from "./components";
import { Login } from "../pages/login";
import { Home } from "../pages/home";

export const Router = (): ReactElement => {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route element={<PrivateWrapper />}>
        <Route index element={<Home />} />
      </Route>
    </Routes>
  );
};
