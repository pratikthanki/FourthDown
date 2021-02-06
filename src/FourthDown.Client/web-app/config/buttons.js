// config/buttons.js

import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCompass,
  faMapMarkerAlt,
  faUser,
  faShoppingCart
} from "@fortawesome/free-solid-svg-icons";

const navButtons = [
  {
    label: "Combine",
    path: "/combine",
    icon: <FontAwesomeIcon icon={faCompass} />
  },
  {
    label: "Play by Play",
    path: "/playbyplay",
    icon: <FontAwesomeIcon icon={faMapMarkerAlt} />
  },
  {
    label: "Games",
    path: "/games",
    icon: <FontAwesomeIcon icon={faShoppingCart} />
  },
  {
    label: "Results",
    path: "/results",
    icon: <FontAwesomeIcon icon={faUser} />
  }
];

export default navButtons;