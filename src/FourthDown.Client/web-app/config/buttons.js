import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCompass,
  faMapMarkerAlt,
  faFootballBall,
  faRunning,
  faInfo
} from "@fortawesome/free-solid-svg-icons";

const navButtons = [
  {
    label: "Combine",
    path: "/combine",
    icon: <FontAwesomeIcon icon={faRunning} />
  },
  {
    label: "Play by Play",
    path: "/playbyplay",
    icon: <FontAwesomeIcon icon={faMapMarkerAlt} />
  },
  {
    label: "Games",
    path: "/games",
    icon: <FontAwesomeIcon icon={faFootballBall} />
  },
  {
    label: "Results",
    path: "/results",
    icon: <FontAwesomeIcon icon={faCompass} />
  },
  {
    label: "About",
    path: "/about",
    icon: <FontAwesomeIcon icon={faInfo} />
  }
];

export default navButtons;