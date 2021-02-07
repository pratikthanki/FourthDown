import React, { Component } from 'react';
import Layout from "../components/Layout";
import Dropdown from "../components/Dropdown"
import DataTable, { createTheme } from 'react-data-table-component';
import Card from "@material-ui/core/Card";
import SortIcon from "@material-ui/icons/ArrowDownward";

const axios = require('axios');
const options = [1, 2, 3]
const tableHeader = "Combine Workouts 2020"
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

class CombineView extends Component {
  static async getInitialProps() {
    const res = await axios.get(`https://fourthdown.azurewebsites.net/api/combine?season=2020`);
    return { data: res.data }
  }

  render() {
    return (
      <Layout>
        <Dropdown></Dropdown>

        <div style={{ width: '95%', padding: '20px' }}>
          <Card>
            <DataTable
              title={tableHeader}
              columns={columns}
              data={this.props.data}
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
  }
}

export default CombineView;