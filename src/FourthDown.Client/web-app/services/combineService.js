import { apiUrl } from "../config/appSettings";
import axios from "axios";

export const getCombineSeason = async (season) => {
    const res = await axios.get(`${apiUrl}/api/combine?season=${season}`);
    return res.data;
};