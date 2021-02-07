import React, { useEffect, useState } from 'react';
import Layout from "../components/Layout";
import Dropdown from "../components/SeasonPicker"
import DataTable from 'react-data-table-component';
import Card from "@material-ui/core/Card";
import SortIcon from "@material-ui/icons/ArrowDownward";
import { getCombineSeason } from '../services/combineService';
import { useAppState } from '../hooks/useAppState';

const columns = [
  {
    name: "Name",
    selector: row => row.FirstName + ' ' + row.LastName,
    sortable: true
  },
  {
    name: "College",
    selector: "College",
    sortable: true
  },
  {
    name: "Position",
    selector: "Position",
    sortable: true,
    right: true
  },
  {
    name: "Forty Yard Dash",
    selector: "FORTY_YARD_DASH",
    sortable: true,
    right: true
  },
  {
    name: "Bench Press",
    selector: "BENCH_PRESS",
    sortable: true,
    right: true
  },
  {
    name: "Vertical Jump",
    selector: "VERTICAL_JUMP",
    sortable: true,
    right: true
  },
  {
    name: "Broad Jump",
    selector: "BROAD_JUMP",
    sortable: true,
    right: true
  },
  {
    name: "Three Cone Drill",
    selector: "THREE_CONE_DRILL",
    sortable: true,
    right: true
  },
  {
    name: "Twenty Yard Shuttle",
    selector: "TWENTY_YARD_SHUTTLE",
    sortable: true,
    right: true
  }
];

const CombineView = () => {
  const { season } = useAppState();
  const [combineData, setCombineData] = useState([]);

  useEffect(() => {
    // Not using async/await directly because useEffect does not support it. 
    const fetchCombine = async () => {
      const newCombineData = await getCombineSeason(season);
      setCombineData(newCombineData);
    };

    fetchCombine();
  }, [season]);

  return (
    <Layout>
      <Dropdown></Dropdown>

      <div style={{ width: '95%', padding: '20px' }}>
        <Card>
          <DataTable
            title={`Combine Workouts ${season}`}
            columns={columns}
            data={combineData}
            defaultSortField="LastName"
            sortIcon={<SortIcon />}
            striped="True"
            highlightOnHover="True"
            dense="True"
            fixedHeader="True"
            overflowY="True"
          />
        </Card>
      </div>
    </Layout>
  );
};

export default CombineView;