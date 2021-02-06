import React from 'react'
import Head from 'next/head'
import Layout from "../components/Layout";

const axios = require('axios');

export default class extends React.Component {
  static async getInitialProps() {
    const res = await axios.get('https://fourthdown.azurewebsites.net/api/combine?season=2020');
    return { data: res.data }
  }

  render() {
    return (
      <Layout>
        <div>
          <div className="pure-g">
            <div className="pure-u-1-3">
              <h1>Combine Workouts</h1>
              <table className="pure-table">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>College</th>
                    <th>Position</th>
                    <th>Season</th>
                    <th>Forty Yard Dash</th>
                    <th>Bench Press</th>
                    <th>Vertical Jump</th>
                    <th>Broad Jump</th>
                    <th>Three Cone Drill</th>
                    <th>Twenty Yard Shuttle</th>
                  </tr>
                </thead>
                <tbody>
                  {this.props.data.map((workout, i) => {
                    const oddOrNot = i % 2 == 1 ? "pure-table-odd" : "";
                    return (
                      <tr key={i} className={oddOrNot}>
                        <td>{workout.FirstName + ' ' + workout.LastName}</td>
                        <td>{workout.College}</td>
                        <td>{workout.Position}</td>
                        <td>{workout.Season}</td>
                        <td>{workout.FORTY_YARD_DASH}</td>
                        <td>{workout.BENCH_PRESS}</td>
                        <td>{workout.VERTICAL_JUMP}</td>
                        <td>{workout.BROAD_JUMP}</td>
                        <td>{workout.THREE_CONE_DRILL}</td>
                        <td>{workout.TWENTY_YARD_SHUTTLE}</td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </Layout>
    );
  }
}