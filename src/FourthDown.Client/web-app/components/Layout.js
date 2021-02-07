import Head from "next/head";

import Header from "./Header";
import NavBar from "./NavBar";

import styles from "./Layout.module.scss";

import navButtons from "../config/buttons";

const Layout = props => {
  const appTitle = `> FourthDown Analytics`;

  return (
    <div className={styles.Layout}>
      <Head>
        <title>FourthDown</title>
      </Head>

      <Header appTitle={appTitle} />
      <NavBar navButtons={navButtons} />
      <div className={styles.Content}>{props.children}</div>
    </div>
  );
};

export default Layout;
