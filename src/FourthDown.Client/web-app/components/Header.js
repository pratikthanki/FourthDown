import Link from "next/link";

import styles from "./Header.module.scss";

const Header = props => (
  <Link href="/">
    <div className={styles.Header}>{props.appTitle}</div>
  </Link>
);

export default Header;
