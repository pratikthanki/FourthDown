import PropTypes from "prop-types";

const TeamType = PropTypes.shape({
  city: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  abr: PropTypes.string.isRequired,
  conf: PropTypes.string.isRequired,
  div: PropTypes.string.isRequired,
  label: PropTypes.string.isRequired,
  teamNameLabel: PropTypes.string.isRequired,

});

export default TeamType;
