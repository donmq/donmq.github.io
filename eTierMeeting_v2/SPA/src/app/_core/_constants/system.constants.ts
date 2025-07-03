export class MessageConstants {
    public static LOGGED_IN = 'Welcome to the system';
    public static LOGIN_FAILED = 'Your username and/or password do not match';

    public static LOGGED_OUT = 'You have been logged out';

    public static CONFIRM_DELETE = 'Are you sure delete?';
    public static CONFIRM_DELETE_MSG = 'Are you sure you want to delete this record?';
    public static CONFIRM_DELETE_RANGE_MSG = 'Are you sure you want to delete these records?';

    public static CREATED_OK_MSG = 'Create successfully';
    public static UPDATED_OK_MSG = 'Update successfully';
    public static DELETED_OK_MSG = 'Delete successfully';
    public static UPLOAD_OK_MSG = 'Upload successful';

    public static CREATED_ERROR_MSG = 'Creating failed on save';
    public static UPDATED_ERROR_MSG = 'Updating failed on save';
    public static DELETED_ERROR_MSG = 'Deleting failed on save';
    public static UPLOAD_ERROR_MSG = 'Uploading failed on save';

    public static UNAUTHORIZED = 'You are not authorized to view this page';

    public static UN_KNOWN_ERROR = 'Oops! Sorry, an error occurred while processing your request';
    public static SYSTEM_ERROR_MSG = 'An error occurred while connecting to the server';

    public static NO_DATA = 'Data Not Found';

    public static INVALID_FILE = 'Please select a file';
    public static ALLOW_IMAGE_FILE = 'Allowed file extensions are .jpg, .png, .jpeg';
    public static ALLOW_VIDEO_FILE = 'Allowed file extensions are .mp4';
    public static FILE_IMAGE_SIZE = 'Image size is too big';
    public static FILE_VIDEO_SIZE = 'Video size is too big';

    public static SELECT_DATE = 'Please select From Date and To Date';
    public static SELECT_FORMAT_DATE = 'Please enter the date in format "YYYY/MM/DD"';
    public static COMPARE_DATE = 'From Date cannot be greater than To Date';

    public static SELECT_RECORD = 'Please select at least 1 record to delete!';

    public static QUERY_SUCCESS = 'Query successfully';
    public static QUERY_ERROR = 'Query Error';
    public static SELECT_ALL_QUERY_OPTION = 'Please select all query options';
    public static SELECT_QUERY_OPTIONS = 'Please select the necessary options (*) to query';
    public static QUERY_SEARCH = 'Please search data';


    public static CLEAR = 'Clear successfully';
}

export class CaptionConstants {
    public static LOGIN_FAILED = 'Login Failed!';
    public static SUCCESS = 'Success!';
    public static ERROR = 'Error!';
    public static WARNING = 'Warning!';
    public static UNAUTHORIZED = 'Unauthorized!';
}

export class ActionConstants {
    public static CREATE = 'CREATE';
    public static EDIT = 'EDIT';
    public static DELETE = 'DELETE';
    public static VIEW = 'VIEW';
    public static APPROVAL = 'APPROVAL';
    public static EXCEL_EXPORT = 'EXCEL_EXPORT';
    public static EXCEL_IMPORT = 'EXCEL_IMPORT';
    public static PRINT = 'PRINT';
    public static DONE = 'DONE';
    public static FINISH = 'FINISH';
    public static RELEASE = 'RELEASE';
}