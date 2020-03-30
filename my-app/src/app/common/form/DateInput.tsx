import React from "react";
import { FieldRenderProps } from "react-final-form";
import { FormFieldProps, Form, Label, Select } from "semantic-ui-react";
import { DateTimePicker } from "react-widgets";

interface IProps extends FieldRenderProps<Date, HTMLElement>, FormFieldProps {}

 const DateInput: React.FC<IProps> = ({
  input,
  width,
  id = null,
  placeholder,
  date = false,
  time = false,
  meta: { touched, error },
  ...rest //da imamo sve propretije za datetimepicker
}) => {
  return (
    <Form.Field error={touched && !!error} width={width}>
      <DateTimePicker
        placeholder={placeholder}
        value={input.value || null}
        onChange={input.onChange}
        date={date}
        time={time}
        onBlur={input.onBlur}
        onKeyDown={(e)=>e.preventDefault()}
        {...rest}
      />
      {touched && error && (
        <Label basic color="red">
          {error}
        </Label>
      )}
    </Form.Field>
  );
};
export default DateInput;