import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';

const useStyles = makeStyles((theme) => ({
    margin: {
        margin: theme.spacing(1),
    },
}));

export default function SimpleSelect() {
    const classes = useStyles();
    const [seasons, setSeasons] = React.useState('');

    const handleChange = (event) => {
        console.log(event.target.value);
        setSeasons(event.target.value);
    };

    return (
        <div style={{ width: '20%', padding: '10px' }}>
            <FormControl variant="outlined" className={classes.margin} fullWidth={true}>
                <InputLabel id="demo-simple-select-outlined-label">Season</InputLabel>
                <Select
                    labelId="demo-simple-select-outlined-label"
                    id="demo-simple-select-outlined"
                    value={seasons}
                    onChange={handleChange}
                    label="Season"
                >
                    <MenuItem value={2012}>2012</MenuItem>
                    <MenuItem value={2013}>2013</MenuItem>
                    <MenuItem value={2014}>2014</MenuItem>
                    <MenuItem value={2015}>2015</MenuItem>
                    <MenuItem value={2016}>2016</MenuItem>
                    <MenuItem value={2017}>2017</MenuItem>
                    <MenuItem value={2018}>2018</MenuItem>
                    <MenuItem value={2019}>2019</MenuItem>
                    <MenuItem value={2020}>2020</MenuItem>
                </Select>
            </FormControl>
        </div>
    );
}
