// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Fast.Web.Component;

/// <summary>
/// Component factory - provides unified component creation entry point
/// </summary>
public static class Component
{
    /// <summary>
    /// Create button component
    /// </summary>
    public static ButtonComponent Button()
    {
        return new ButtonComponent();
    }

    /// <summary>
    /// Create input component
    /// </summary>
    public static InputComponent Input()
    {
        return new InputComponent();
    }

    /// <summary>
    /// Create div container component
    /// </summary>
    public static DivComponent Div()
    {
        return new DivComponent();
    }

    /// <summary>
    /// Create form component
    /// </summary>
    public static FormComponent Form()
    {
        return new FormComponent();
    }

    /// <summary>
    /// Create span component
    /// </summary>
    public static SpanComponent Span()
    {
        return new SpanComponent();
    }

    /// <summary>
    /// Create label component
    /// </summary>
    public static LabelComponent Label()
    {
        return new LabelComponent();
    }

    /// <summary>
    /// Create textarea component
    /// </summary>
    public static TextareaComponent Textarea()
    {
        return new TextareaComponent();
    }

    /// <summary>
    /// Create select component
    /// </summary>
    public static SelectComponent Select()
    {
        return new SelectComponent();
    }

    /// <summary>
    /// Create table component
    /// </summary>
    public static TableComponent Table()
    {
        return new TableComponent();
    }

    /// <summary>
    /// Create card component
    /// </summary>
    public static CardComponent Card()
    {
        return new CardComponent();
    }

    /// <summary>
    /// Create message component
    /// </summary>
    public static MessageComponent Message()
    {
        return new MessageComponent();
    }

    /// <summary>
    /// Create modal dialog component
    /// </summary>
    public static ModalComponent Modal()
    {
        return new ModalComponent();
    }

    /// <summary>
    /// Create tabs component
    /// </summary>
    public static TabsComponent Tabs()
    {
        return new TabsComponent();
    }

    /// <summary>
    /// Create accordion component
    /// </summary>
    public static AccordionComponent Accordion()
    {
        return new AccordionComponent();
    }

    /// <summary>
    /// Create calendar component
    /// </summary>
    public static CalendarComponent Calendar()
    {
        return new CalendarComponent();
    }

    /// <summary>
    /// Create tree component
    /// </summary>
    public static TreeComponent Tree()
    {
        return new TreeComponent();
    }

    /// <summary>
    /// Create chart component
    /// </summary>
    public static ChartComponent Chart()
    {
        return new ChartComponent();
    }

    /// <summary>
    /// Create file upload component
    /// </summary>
    public static FileUploadComponent FileUpload()
    {
        return new FileUploadComponent();
    }

    /// <summary>
    /// Create rich text editor component
    /// </summary>
    public static RichTextComponent RichText()
    {
        return new RichTextComponent();
    }
}
