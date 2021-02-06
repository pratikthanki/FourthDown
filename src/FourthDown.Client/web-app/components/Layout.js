// components/Layout.js
import Head from "next/head";

import Header from "./Header";
import NavBar from "./NavBar";

import "./Layout.scss";
import "./index.scss";

import navButtons from "../config/buttons";

const Layout = props => {
  const appTitle = `> FourthDown Analytics`;

  return (
    <div className="Layout">
      <Head>
        <title>FourthDown</title>
      </Head>

      <Header appTitle={appTitle} />
      <NavBar navButtons={navButtons} />
      <div className="Content">{props.children}</div>
    </div>
  );
};

export default Layout;
