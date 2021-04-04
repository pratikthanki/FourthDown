import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import { useAppState } from '../hooks/useAppState';

const useStyles = makeStyles((theme) => ({
    margin: {
        margin: theme.spacing(1),
    },
}));

const seasons = [
    2012,
    2013,
    2014,
    2015,
    2016,
    2017,
    2018,
    2019,
    2020
];

const SeasonPicker = () => {
    const classes = useStyles();
    const { season, setSeason } = useAppState();

    const handleChange = (event) => {
        setSeason(event.target.value);
    };

    return (
        <div style={{ width: '20%', padding: '10px' }}>
            <FormControl variant="outlined" className={classes.margin} fullWidth={true}>
                <InputLabel id="demo-simple-select-outlined-label">Season</InputLabel>
                <Select
                    labelId="demo-simple-select-outlined-label"
                    id="demo-simple-select-outlined"
                    value={season}
                    onChange={handleChange}
                    label="Season"
                >
                    {
                        seasons.map(s => (
                            <MenuItem key={s} value={s}>{s}</MenuItem>
                        ))    
                    }
                </Select>
            </FormControl>
        </div>
    );
};

export default SeasonPicker;
