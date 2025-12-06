import type { JSX } from "react";
import SideBar from "../components/AdminComponents/SideBar";
import { Outlet } from "react-router-dom";

function Admin(): JSX.Element {
    return (
        <>
            <div>
                <SideBar />
            </div>
            <div>
                <h1>Ez itt az admin page. Használd egészséggel</h1>
                <Outlet />
            </div>
        </>
    );
}

export default Admin;
