import type { JSX } from "react";
import Draw from "./AdminChildPages/Draw";
import SideBar from "../components/AdminComponents/SideBar";

function Admin(): JSX.Element {
    return (
        <>
            <div>
                <SideBar />
            </div>
            <div>
                <h1>Ez itt az admin page. Használd egészséggel</h1>
                <Draw />
            </div>
        </>
    );
}

export default Admin;
