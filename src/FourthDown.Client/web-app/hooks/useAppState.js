import { useContext } from "react";
import { AppStateContext } from "../providers/AppStateProvider";

export const useAppState = () => useContext(AppStateContext);